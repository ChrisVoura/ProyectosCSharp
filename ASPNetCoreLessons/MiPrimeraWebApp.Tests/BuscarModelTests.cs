using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiPrimeraWebApp.Data;
using MiPrimeraWebApp.Pages;
using Moq;

namespace MiPrimeraWebApp.Tests;

public class BuscarModelTests : IDisposable
{
    private readonly AppDbContext _db;
    private readonly BuscarModel _model;

    public BuscarModelTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _db = new AppDbContext(options);
        
        var httpContext = new DefaultHttpContext();
        _model = new BuscarModel(_db)
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
    public void OnGet_SearchByName_Found()
    {
        _db.Productos.Add(new Producto { Name = "Manzana", Price = 5.00m, Category = "Frutas", Description = "Fruta roja", ImageUrl = "url1" });
        _db.Productos.Add(new Producto { Name = "Naranja", Price = 3.00m, Category = "Frutas", Description = "Fruta naranja", ImageUrl = "url2" });
        _db.SaveChanges();

        _model.PageContext.HttpContext.Request.QueryString = new QueryString("?query=Manzana");

        _model.OnGet();

        Assert.Single(_model.Resultados);
        Assert.Equal("Manzana", _model.Resultados[0].Name);
    }

    [Fact]
    public void OnGet_SearchByDescription_Found()
    {
        _db.Productos.Add(new Producto { Name = "Producto Especial", Price = 10.00m, Category = "Cat", Description = "Descripción", ImageUrl = "url" });
        _db.Productos.Add(new Producto { Name = "Producto B", Price = 20.00m, Category = "Cat", Description = "Otra descripción diferente", ImageUrl = "url" });
        _db.SaveChanges();

        _model.PageContext.HttpContext.Request.QueryString = new QueryString("?query=especial");

        _model.OnGet();

        Assert.Single(_model.Resultados);
        Assert.Equal("Producto Especial", _model.Resultados[0].Name);
    }

    [Fact]
    public void OnGet_SearchCaseInsensitive()
    {
        _db.Productos.Add(new Producto { Name = "MANZANA", Price = 5.00m, Category = "Frutas", Description = "Fruta", ImageUrl = "url" });
        _db.SaveChanges();

        _model.PageContext.HttpContext.Request.QueryString = new QueryString("?query=manzana");

        _model.OnGet();

        Assert.Single(_model.Resultados);
    }

    [Fact]
    public void OnGet_ReturnEmpty_WhenNoQueryParameter()
    {
        _db.Productos.Add(new Producto { Name = "Producto", Price = 10.00m, Category = "Cat", ImageUrl = "url" });
        _db.SaveChanges();

        _model.PageContext.HttpContext.Request.QueryString = new QueryString("");

        _model.OnGet();

        Assert.Empty(_model.Resultados);
        Assert.False(_model.ModelState.IsValid);
    }

    [Fact]
    public void OnGet_ReturnEmpty_WhenNoResults()
    {
        _db.Productos.Add(new Producto { Name = "Producto1", Price = 10.00m, Category = "Cat1", ImageUrl = "url1" });
        _db.SaveChanges();

        _model.PageContext.HttpContext.Request.QueryString = new QueryString("?query=noexiste");

        _model.OnGet();

        Assert.Empty(_model.Resultados);
        Assert.False(_model.ModelState.IsValid);
    }

    [Fact]
    public void OnGet_ReturnMultipleResults_WhenMultipleMatches()
    {
        _db.Productos.Add(new Producto { Name = "Manzana Verde", Price = 5.00m, Category = "Frutas", Description = "Fruta verde", ImageUrl = "url1" });
        _db.Productos.Add(new Producto { Name = "Manzana Roja", Price = 5.50m, Category = "Frutas", Description = "Fruta roja", ImageUrl = "url2" });
        _db.Productos.Add(new Producto { Name = "Naranja", Price = 3.00m, Category = "Frutas", Description = "Fruta", ImageUrl = "url3" });
        _db.SaveChanges();

        _model.PageContext.HttpContext.Request.QueryString = new QueryString("?query=manzana");

        _model.OnGet();

        Assert.Equal(2, _model.Resultados.Count);
    }

    [Fact]
    public void OnGet_HandleEmptyQueryValue()
    {
        _model.PageContext.HttpContext.Request.QueryString = new QueryString("?query=");

        _model.OnGet();

        Assert.Empty(_model.Resultados);
    }
}
