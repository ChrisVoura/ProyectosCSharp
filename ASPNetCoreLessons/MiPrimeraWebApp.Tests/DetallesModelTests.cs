using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MiPrimeraWebApp.Data;
using MiPrimeraWebApp.Pages;

namespace MiPrimeraWebApp.Tests;

public class DetallesModelTests : IDisposable
{
    private readonly AppDbContext _db;
    private readonly DetallesModel _model;

    public DetallesModelTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _db = new AppDbContext(options);
        
        var httpContext = new DefaultHttpContext();
        _model = new DetallesModel(_db)
        {
            PageContext = new PageContext
            {
                HttpContext = httpContext
            }
        };
    }

    public void Dispose()
    {
        _db.Database.EnsureDeleted();
        _db.Dispose();
    }

    [Fact]
    public void OnGet_ReturnProducto_WhenExists()
    {
        var producto = new Producto 
        { 
            Name = "Producto Test", 
            Price = 25.00m, 
            Category = "Test",
            Description = "Descripción test",
            ImageUrl = "url/test.jpg"
        };
        _db.Productos.Add(producto);
        _db.SaveChanges();

        _model.PageContext.HttpContext.Request.QueryString = new QueryString($"?id={producto.Id}");

        _model.OnGet();

        Assert.NotNull(_model.Producto);
        Assert.Equal("Producto Test", _model.Producto.Name);
        Assert.Equal(25.00m, _model.Producto.Price);
    }

    [Fact]
    public void OnGet_ReturnNull_WhenIdNotExists()
    {
        _model.PageContext.HttpContext.Request.QueryString = new QueryString("?id=999");

        _model.OnGet();

        Assert.Null(_model.Producto);
    }

    [Fact]
    public void OnGet_ReturnNull_WhenNoIdParameter()
    {
        _model.PageContext.HttpContext.Request.QueryString = new QueryString("");

        _model.OnGet();

        Assert.Null(_model.Producto);
    }

    [Fact]
    public void OnGet_HandleInvalidIdFormat()
    {
        _model.PageContext.HttpContext.Request.QueryString = new QueryString("?id=invalid");

        _model.OnGet();

        Assert.Null(_model.Producto);
    }

    [Fact]
    public void OnGet_ReturnCorrectProducto_WhenMultipleExist()
    {
        _db.Productos.Add(new Producto { Name = "Producto1", Price = 10.00m, Category = "Cat1", ImageUrl = "url1" });
        _db.Productos.Add(new Producto { Name = "Producto2", Price = 20.00m, Category = "Cat2", ImageUrl = "url2" });
        _db.Productos.Add(new Producto { Name = "Producto3", Price = 30.00m, Category = "Cat3", ImageUrl = "url3" });
        _db.SaveChanges();

        var producto2 = _db.Productos.First(p => p.Name == "Producto2");
        _model.PageContext.HttpContext.Request.QueryString = new QueryString($"?id={producto2.Id}");

        _model.OnGet();

        Assert.NotNull(_model.Producto);
        Assert.Equal("Producto2", _model.Producto.Name);
        Assert.Equal(20.00m, _model.Producto.Price);
    }
}
