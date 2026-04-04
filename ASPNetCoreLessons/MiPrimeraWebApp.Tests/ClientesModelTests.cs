using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MiPrimeraWebApp.Data;
using MiPrimeraWebApp.Pages;
using Moq;

namespace MiPrimeraWebApp.Tests;

public class ClientesModelTests : IDisposable
{
    private readonly AppDbContext _db;
    private readonly ClientesModel _model;

    public ClientesModelTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _db = new AppDbContext(options);
        _model = CreateModelWithTempData();
    }

    private ClientesModel CreateModelWithTempData()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Session = new TestSession();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        
        var model = new ClientesModel(_db)
        {
            PageContext = new PageContext
            {
                HttpContext = httpContext
            }
        };
        model.TempData = tempData;
        
        return model;
    }

    public void Dispose()
    {
        _db.Database.EnsureDeleted();
        _db.Dispose();
    }

    [Fact]
    public async Task OnGetAsync_ReturnAllClientesOrderedByName()
    {
        _db.Clientes.Add(new Cliente { Name = "Carlos", Apellido = "Gomez", Email = "carlos@test.com", Password = "test123", FechaRegistro = DateTime.Now });
        _db.Clientes.Add(new Cliente { Name = "Ana", Apellido = "Lopez", Email = "ana@test.com", Password = "test123", FechaRegistro = DateTime.Now });
        _db.Clientes.Add(new Cliente { Name = "Pedro", Apellido = "Perez", Email = "pedro@test.com", Password = "test123", FechaRegistro = DateTime.Now });
        await _db.SaveChangesAsync();

        var httpContext = new DefaultHttpContext();
        httpContext.Session = new TestSession();
        httpContext.Session.SetString("UsuarioId", "1");
        
        var model = new ClientesModel(_db)
        {
            PageContext = new PageContext { HttpContext = httpContext }
        };

        await model.OnGetAsync(null);

        Assert.Equal(3, model.Clientes.Count);
        Assert.Equal("Ana", model.Clientes[0].Name);
        Assert.Equal("Carlos", model.Clientes[1].Name);
        Assert.Equal("Pedro", model.Clientes[2].Name);
    }

    [Fact]
    public async Task OnGetAsync_ReturnEmptyList_WhenNoClientes()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Session = new TestSession();
        httpContext.Session.SetString("UsuarioId", "1");
        
        var model = new ClientesModel(_db)
        {
            PageContext = new PageContext { HttpContext = httpContext }
        };

        await model.OnGetAsync(null);

        Assert.Empty(model.Clientes);
    }

    [Fact]
    public async Task OnGetAsync_RedirectToLogin_WhenNotLoggedIn()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Session = new TestSession();
        
        var model = new ClientesModel(_db)
        {
            PageContext = new PageContext { HttpContext = httpContext }
        };

        await model.OnGetAsync(null);

        Assert.Empty(model.Clientes);
    }

    [Fact]
    public async Task OnPostDelete_RemoveCliente_WhenExists()
    {
        var cliente = new Cliente 
        { 
            Name = "ParaEliminar", 
            Apellido = "Elim",
            Email = "eliminar@test.com", 
            Password = "test123",
            FechaRegistro = DateTime.Now 
        };
        _db.Clientes.Add(cliente);
        await _db.SaveChangesAsync();

        var result = await _model.OnPostDelete(cliente.Id);

        Assert.Empty(_db.Clientes);
        Assert.IsType<RedirectToPageResult>(result);
    }

    [Fact]
    public async Task OnPostDelete_DoesNotThrow_WhenClienteNotExists()
    {
        var result = await _model.OnPostDelete(999);

        Assert.Empty(_db.Clientes);
        Assert.IsType<RedirectToPageResult>(result);
    }
}
