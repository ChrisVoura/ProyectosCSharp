using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MiPrimeraWebApp.Data;
using MiPrimeraWebApp.Pages;
using System.Text.Json;

namespace MiPrimeraWebApp.Tests;

public class CarritoModelTests : IDisposable
{
    private readonly AppDbContext _db;
    private readonly CarritoModel _model;

    public CarritoModelTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _db = new AppDbContext(options);
        
        var httpContext = new DefaultHttpContext();
        httpContext.Session = new TestSession();
        
        _model = new CarritoModel(_db)
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
    public void OnGet_ReturnEmptyList_WhenCarritoIsEmpty()
    {
        _model.OnGet();

        Assert.Empty(_model.ProductosEnCarrito);
    }

    [Fact]
    public void OnGet_ReturnProductos_WhenCarritoHasItems()
    {
        var producto = new Producto { Name = "Producto1", Price = 10.00m, Category = "Cat", ImageUrl = "url" };
        _db.Productos.Add(producto);
        _db.SaveChanges();

        var httpContext = new DefaultHttpContext();
        httpContext.Session = new TestSession();
        httpContext.Session.SetString("Carrito", JsonSerializer.Serialize(new Dictionary<int, int> { { producto.Id, 2 } }));
        
        var model = new CarritoModel(_db)
        {
            PageContext = new PageContext { HttpContext = httpContext }
        };

        model.OnGet();

        Assert.Single(model.ProductosEnCarrito);
        Assert.Equal("Producto1", model.ProductosEnCarrito[0].Name);
    }

    [Fact]
    public void OnGetCarritoCount_ReturnZero_WhenEmpty()
    {
        var result = _model.OnGetCarritoCount();
        Assert.IsType<JsonResult>(result);
    }

    [Fact]
    public void OnGetCarritoCount_ReturnCorrectCount()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Session = new TestSession();
        httpContext.Session.SetString("Carrito", JsonSerializer.Serialize(new Dictionary<int, int> { { 1, 2 }, { 2, 3 } }));
        
        var model = new CarritoModel(_db)
        {
            PageContext = new PageContext { HttpContext = httpContext }
        };

        var result = model.OnGetCarritoCount();
        Assert.IsType<JsonResult>(result);
    }

    [Fact]
    public void OnPostAgregar_AddNewItem()
    {
        var producto = new Producto { Name = "Producto1", Price = 10.00m, Category = "Cat", ImageUrl = "url" };
        _db.Productos.Add(producto);
        _db.SaveChanges();

        var httpContext = new DefaultHttpContext();
        httpContext.Session = new TestSession();
        httpContext.Request.Headers["Referer"] = "http://localhost/test";
        
        var model = new CarritoModel(_db)
        {
            PageContext = new PageContext { HttpContext = httpContext }
        };

        var result = model.OnPostAgregar(producto.Id);

        Assert.IsType<RedirectResult>(result);
    }

    [Fact]
    public void OnPostAgregar_IncrementExistingItem()
    {
        var producto = new Producto { Name = "Producto1", Price = 10.00m, Category = "Cat", ImageUrl = "url" };
        _db.Productos.Add(producto);
        _db.SaveChanges();

        var httpContext = new DefaultHttpContext();
        httpContext.Session = new TestSession();
        httpContext.Session.SetString("Carrito", JsonSerializer.Serialize(new Dictionary<int, int> { { producto.Id, 1 } }));
        httpContext.Request.Headers["Referer"] = "http://localhost/test";
        
        var model = new CarritoModel(_db)
        {
            PageContext = new PageContext { HttpContext = httpContext }
        };

        model.OnPostAgregar(producto.Id);

        var session = (TestSession)httpContext.Session;
        var carrito = JsonSerializer.Deserialize<Dictionary<int, int>>(session.GetString("Carrito"));
        Assert.Equal(2, carrito[producto.Id]);
    }

    [Fact]
    public void OnGetLimpiar_RemovesCarrito()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Session = new TestSession();
        httpContext.Session.SetString("Carrito", JsonSerializer.Serialize(new Dictionary<int, int> { { 1, 2 } }));
        
        var model = new CarritoModel(_db)
        {
            PageContext = new PageContext { HttpContext = httpContext }
        };

        var result = model.OnGetLimpiar();

        Assert.IsType<JsonResult>(result);
        Assert.Null(httpContext.Session.GetString("Carrito"));
    }

    [Fact]
    public void OnGetCambiarCantidad_IncrementCantidad()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Session = new TestSession();
        httpContext.Session.SetString("Carrito", JsonSerializer.Serialize(new Dictionary<int, int> { { 1, 2 } }));
        
        var model = new CarritoModel(_db)
        {
            PageContext = new PageContext { HttpContext = httpContext }
        };

        model.OnGetCambiarCantidad(1, 1);

        var session = (TestSession)httpContext.Session;
        var carrito = JsonSerializer.Deserialize<Dictionary<int, int>>(session.GetString("Carrito"));
        Assert.Equal(3, carrito[1]);
    }

    [Fact]
    public void OnGetCambiarCantidad_RemoveItem_WhenCantidadIsZero()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Session = new TestSession();
        httpContext.Session.SetString("Carrito", JsonSerializer.Serialize(new Dictionary<int, int> { { 1, 1 } }));
        
        var model = new CarritoModel(_db)
        {
            PageContext = new PageContext { HttpContext = httpContext }
        };

        model.OnGetCambiarCantidad(1, -1);

        var session = (TestSession)httpContext.Session;
        var carrito = JsonSerializer.Deserialize<Dictionary<int, int>>(session.GetString("Carrito"));
        Assert.False(carrito.ContainsKey(1));
    }

    [Fact]
    public void OnGetEliminarProducto_RemovesItem()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Session = new TestSession();
        httpContext.Session.SetString("Carrito", JsonSerializer.Serialize(new Dictionary<int, int> { { 1, 2 }, { 2, 1 } }));
        
        var model = new CarritoModel(_db)
        {
            PageContext = new PageContext { HttpContext = httpContext }
        };

        model.OnGetEliminarProducto(1);

        var session = (TestSession)httpContext.Session;
        var carrito = JsonSerializer.Deserialize<Dictionary<int, int>>(session.GetString("Carrito"));
        Assert.False(carrito.ContainsKey(1));
        Assert.True(carrito.ContainsKey(2));
    }
}

public class TestSession : ISession
{
    private readonly Dictionary<string, byte[]> _store = new();

    public bool IsAvailable => true;
    public string Id => "test";
    public IEnumerable<string> Keys => _store.Keys;

    public void Clear() => _store.Clear();
    public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public void Set(string key, byte[] value) => _store[key] = value;
    public void Remove(string key) => _store.Remove(key);

    public bool TryGetValue(string key, out byte[]? value)
    {
        var exists = _store.TryGetValue(key, out var bytes);
        value = bytes;
        return exists;
    }

    public byte[]? Get(string key) => _store.TryGetValue(key, out var bytes) ? bytes : null;
    public string? GetString(string key)
    {
        var bytes = Get(key);
        return bytes != null ? System.Text.Encoding.UTF8.GetString(bytes) : null;
    }

    public void SetString(string key, string value) => Set(key, System.Text.Encoding.UTF8.GetBytes(value));
}
