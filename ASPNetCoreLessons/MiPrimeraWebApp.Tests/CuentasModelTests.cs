using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MiPrimeraWebApp.Data;
using MiPrimeraWebApp.Pages;

namespace MiPrimeraWebApp.Tests;

public class CuentasModelTests : IDisposable
{
    private readonly AppDbContext _db;

    public CuentasModelTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _db = new AppDbContext(options);
    }

    public void Dispose()
    {
        _db.Database.EnsureDeleted();
        _db.Dispose();
    }

    [Fact]
    public async Task OnPostCrearLista_CreatesNewList()
    {
        _db.Clientes.Add(new Cliente 
        { 
            Id = 1,
            Name = "Test", 
            Apellido = "User",
            Email = "test@test.com",
            Password = "hash",
            FechaRegistro = DateTime.Now 
        });
        _db.SaveChanges();
        
        var httpContext = new DefaultHttpContext();
        httpContext.Session = new TestSession();
        httpContext.Session.SetString("UsuarioId", "1");
        
        var modelState = new ModelStateDictionary();
        var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor(), modelState);
        
        httpContext.Request.Form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
        {
            { "NuevaListaNombre", "Mi Lista" }
        });
        httpContext.Request.Headers["Referer"] = "/Cuentas";

        var model = new CuentasModel(_db)
        {
            PageContext = new PageContext(actionContext)
        };

        var form = await httpContext.Request.ReadFormAsync();
        model.NuevaListaNombre = form["NuevaListaNombre"].ToString();
        
        var result = model.OnPostCrearLista();
        
        Assert.IsType<RedirectToPageResult>(result);
        Assert.Single(_db.ListasDeseos);
        Assert.Equal("Mi Lista", _db.ListasDeseos.First().Nombre);
    }

    [Fact]
    public void OnPostAgregarAListaEspecifica_AddsProductToList()
    {
        _db.Clientes.Add(new Cliente 
        { 
            Id = 1,
            Name = "Test", 
            Apellido = "User",
            Email = "test@test.com",
            Password = "hash",
            FechaRegistro = DateTime.Now 
        });
        
        var lista = new ListaDeseo 
        { 
            Id = 1, 
            Nombre = "Mi Lista", 
            ClienteId = 1, 
            Productos = "" 
        };
        _db.ListasDeseos.Add(lista);
        
        _db.Productos.Add(new Producto 
        { 
            Id = 1, 
            Name = "Test Product", 
            Price = 10.00m, 
            Category = "Test",
            ImageUrl = "url"
        });
        
        _db.SaveChanges();

        var httpContext = new DefaultHttpContext();
        httpContext.Session = new TestSession();
        httpContext.Session.SetString("UsuarioId", "1");
        httpContext.Request.Form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
        {
            { "ProductoId", "1" },
            { "ListaId", "1" }
        });

        var model = new CuentasModel(_db)
        {
            PageContext = new PageContext { HttpContext = httpContext }
        };

        var result = model.OnPostAgregarAListaEspecifica();
        
        Assert.IsType<JsonResult>(result);
    }

    [Fact]
    public void OnPostAgregarListaAlCarrito_AddsAllProductsToCart()
    {
        _db.Clientes.Add(new Cliente 
        { 
            Id = 1,
            Name = "Test", 
            Apellido = "User",
            Email = "test@test.com",
            Password = "hash",
            FechaRegistro = DateTime.Now 
        });
        
        var lista = new ListaDeseo 
        { 
            Id = 1, 
            Nombre = "Mi Lista", 
            ClienteId = 1, 
            Productos = "1,2" 
        };
        _db.ListasDeseos.Add(lista);
        
        _db.Productos.Add(new Producto { Id = 1, Name = "P1", Price = 10, Category = "C", ImageUrl = "" });
        _db.Productos.Add(new Producto { Id = 2, Name = "P2", Price = 20, Category = "C", ImageUrl = "" });
        
        _db.SaveChanges();

        var httpContext = new DefaultHttpContext();
        httpContext.Session = new TestSession();
        httpContext.Session.SetString("UsuarioId", "1");
        httpContext.Request.Form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
        {
            { "ListaId", "1" }
        });

        var model = new CuentasModel(_db)
        {
            PageContext = new PageContext { HttpContext = httpContext }
        };

        var result = model.OnPostAgregarListaAlCarrito();
        
        Assert.IsType<JsonResult>(result);
    }
}