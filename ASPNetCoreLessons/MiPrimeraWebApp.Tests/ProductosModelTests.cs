using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MiPrimeraWebApp.Data;
using MiPrimeraWebApp.Pages;

namespace MiPrimeraWebApp.Tests;

public class ProductosModelTests : IDisposable
{
    private readonly AppDbContext _db;
    private readonly ProductosModel _model;

    public ProductosModelTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _db = new AppDbContext(options);
        _model = new ProductosModel(_db);
    }

    public void Dispose()
    {
        _db.Database.EnsureDeleted();
        _db.Dispose();
    }

    [Fact]
    public void OnGet_ReturnAllProductos()
    {
        _db.Productos.Add(new Producto { Name = "Producto1", Price = 10.00m, Category = "Cat1", ImageUrl = "url1" });
        _db.Productos.Add(new Producto { Name = "Producto2", Price = 20.00m, Category = "Cat2", ImageUrl = "url2" });
        _db.SaveChanges();

        _model.OnGet();

        Assert.Equal(2, _model.Productos.Count);
    }

    [Fact]
    public void OnGet_AddModelError_WhenNoProductos()
    {
        _model.OnGet();

        Assert.Empty(_model.Productos);
        Assert.False(_model.ModelState.IsValid);
    }

    [Fact]
    public async Task OnPost_AddNewProducto_WithSingleImageUrl()
    {
        _model.Nombre = "Nuevo Producto";
        _model.Precio = 25.50m;
        _model.Descripcion = "Descripcion del producto";
        _model.Categoria = "Categoria1";
        _model.ImageUrl1 = "https://example.com/image1.jpg";
        _model.ImageUrl2 = null;
        _model.ImageUrl3 = null;

        var result = await _model.OnPost();

        Assert.Single(_db.Productos);
        var producto = _db.Productos.First();
        Assert.Equal("Nuevo Producto", producto.Name);
        Assert.Equal(25.50m, producto.Price);
        Assert.Equal("https://example.com/image1.jpg", producto.ImageUrl);
    }

    [Fact]
    public async Task OnPost_AddNewProducto_WithMultipleImageUrls()
    {
        _model.Nombre = "Producto Multiple";
        _model.Precio = 50.00m;
        _model.Descripcion = "Descripcion";
        _model.Categoria = "Categoria1";
        _model.ImageUrl1 = "https://example.com/image1.jpg";
        _model.ImageUrl2 = "https://example.com/image2.jpg";
        _model.ImageUrl3 = "https://example.com/image3.jpg";

        await _model.OnPost();

        var producto = _db.Productos.First();
        Assert.Equal("https://example.com/image1.jpg,https://example.com/image2.jpg,https://example.com/image3.jpg", producto.ImageUrl);
    }

    [Fact]
    public async Task OnPost_CombineEmptyAndNonEmptyImageUrls()
    {
        _model.Nombre = "Producto Mixto";
        _model.Precio = 30.00m;
        _model.Descripcion = "Descripcion";
        _model.Categoria = "Categoria1";
        _model.ImageUrl1 = null;
        _model.ImageUrl2 = "";
        _model.ImageUrl3 = "https://example.com/image3.jpg";

        await _model.OnPost();

        var producto = _db.Productos.First();
        Assert.Equal("https://example.com/image3.jpg", producto.ImageUrl);
    }

    [Fact]
    public void OnPostDelete_RemoveProducto_WhenExists()
    {
        var producto = new Producto { Name = "ParaEliminar", Price = 10.00m, Category = "Cat", ImageUrl = "url" };
        _db.Productos.Add(producto);
        _db.SaveChanges();

        var result = _model.OnPostDelete(producto.Id);

        Assert.Empty(_db.Productos);
        Assert.IsType<RedirectToPageResult>(result);
    }

    [Fact]
    public void OnPostDelete_DoesNotThrow_WhenProductoNotExists()
    {
        var result = _model.OnPostDelete(999);

        Assert.Empty(_db.Productos);
        Assert.IsType<RedirectToPageResult>(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    [InlineData(0.01)]
    [InlineData(999999.99)]
    public async Task OnPost_AcceptsVariousPrices(decimal price)
    {
        _model.Nombre = "Producto";
        _model.Precio = price;
        _model.Descripcion = "Desc";
        _model.Categoria = "Cat";
        _model.ImageUrl1 = "url";

        await _model.OnPost();

        Assert.Single(_db.Productos);
        Assert.Equal(price, _db.Productos.First().Price);
    }
}
