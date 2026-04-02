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
        _db.Clientes.Add(new Cliente { Name = "Carlos", Email = "carlos@test.com", FechaRegistro = DateTime.Now });
        _db.Clientes.Add(new Cliente { Name = "Ana", Email = "ana@test.com", FechaRegistro = DateTime.Now });
        _db.Clientes.Add(new Cliente { Name = "Pedro", Email = "pedro@test.com", FechaRegistro = DateTime.Now });
        await _db.SaveChangesAsync();

        await _model.OnGetAsync();

        Assert.Equal(3, _model.Clientes.Count);
        Assert.Equal("Ana", _model.Clientes[0].Name);
        Assert.Equal("Carlos", _model.Clientes[1].Name);
        Assert.Equal("Pedro", _model.Clientes[2].Name);
    }

    [Fact]
    public async Task OnGetAsync_ReturnEmptyList_WhenNoClientes()
    {
        await _model.OnGetAsync();

        Assert.Empty(_model.Clientes);
    }

    [Fact]
    public async Task OnPostAsync_AddNewCliente_WhenEmailNotExists()
    {
        _model.NuevoCliente = new Cliente 
        { 
            Name = "Nuevo Cliente", 
            Email = "nuevo@test.com", 
            FechaRegistro = DateTime.Now 
        };

        var result = await _model.OnPostAsync();

        Assert.Single(_db.Clientes);
        Assert.Equal("Nuevo Cliente", _db.Clientes.First().Name);
        Assert.IsType<RedirectToPageResult>(result);
    }

    [Fact]
    public async Task OnPostAsync_ReturnPage_WhenEmailAlreadyExists()
    {
        _db.Clientes.Add(new Cliente 
        { 
            Name = "Existente", 
            Email = "existente@test.com", 
            FechaRegistro = DateTime.Now 
        });
        await _db.SaveChangesAsync();

        _model.NuevoCliente = new Cliente 
        { 
            Name = "Nuevo", 
            Email = "existente@test.com", 
            FechaRegistro = DateTime.Now 
        };

        var result = await _model.OnPostAsync();

        Assert.Single(_db.Clientes);
        Assert.IsType<PageResult>(result);
        Assert.False(_model.ModelState.IsValid);
    }

    [Fact]
    public async Task OnPostAsync_ReturnPage_WhenNuevoClienteIsNull()
    {
        _model.NuevoCliente = null;

        var result = await _model.OnPostAsync();

        Assert.IsType<PageResult>(result);
    }

    [Fact]
    public async Task OnPostDelete_RemoveCliente_WhenExists()
    {
        var cliente = new Cliente 
        { 
            Name = "ParaEliminar", 
            Email = "eliminar@test.com", 
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
