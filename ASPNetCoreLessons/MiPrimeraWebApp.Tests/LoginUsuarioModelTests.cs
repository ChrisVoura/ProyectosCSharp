using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MiPrimeraWebApp.Data;
using MiPrimeraWebApp.Pages;

namespace MiPrimeraWebApp.Tests;

public class LoginModelTests : IDisposable
{
    private readonly AppDbContext _db;
    private readonly LoginModel _model;

    public LoginModelTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _db = new AppDbContext(options);
        
        var httpContext = new DefaultHttpContext();
        httpContext.Session = new TestSession();
        
        _model = new LoginModel(_db)
        {
            PageContext = new PageContext { HttpContext = httpContext }
        };
    }

    public void Dispose()
    {
        _db.Database.EnsureDeleted();
        _db.Dispose();
    }

    [Fact]
    public void OnGet_ReturnDefaultValues()
    {
        _model.OnGet();

        Assert.Equal("", _model.EmailVal);
        Assert.False(_model.ShowPassword);
    }

    [Fact]
    public void OnPostVerificar_ReturnPage_WhenEmailIsEmpty()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>());
        _model.PageContext = new PageContext { HttpContext = httpContext };
        
        var result = _model.OnPostVerificar();
        
        Assert.IsType<PageResult>(result);
    }

    [Fact]
    public void OnPostVerificar_RedirectToUsuario_WhenEmailNotExists()
    {
        var passwordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("test123"));
        
        _db.Clientes.Add(new Cliente 
        { 
            Name = "Test", 
            Apellido = "User",
            Email = "test@test.com",
            Password = passwordHash,
            FechaRegistro = DateTime.Now 
        });
        _db.SaveChanges();
        
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
        {
            { "Email", "otro@test.com" }
        });
        _model.PageContext = new PageContext { HttpContext = httpContext };
        
        var result = _model.OnPostVerificar();
        
        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Usuario", redirect.PageName);
    }

    [Fact]
    public void OnPostVerificar_ReturnPageWithShowPassword_WhenEmailExists()
    {
        var passwordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("test123"));
        
        _db.Clientes.Add(new Cliente 
        { 
            Name = "Test", 
            Apellido = "User",
            Email = "test@test.com",
            Password = passwordHash,
            FechaRegistro = DateTime.Now 
        });
        _db.SaveChanges();
        
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
        {
            { "Email", "test@test.com" }
        });
        _model.PageContext = new PageContext { HttpContext = httpContext };
        
        var result = _model.OnPostVerificar();
        
        Assert.IsType<PageResult>(result);
        Assert.True(_model.ShowPassword);
    }

    [Fact]
    public void OnPost_RedirectToCuentas_WhenValidCredentials()
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("password123");
        
        _db.Clientes.Add(new Cliente 
        { 
            Name = "Test", 
            Apellido = "User",
            Email = "test@test.com",
            Password = passwordHash,
            FechaRegistro = DateTime.Now 
        });
        _db.SaveChanges();
        
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
        {
            { "Email", "test@test.com" },
            { "Password", "password123" }
        });
        httpContext.Session = new TestSession();
        
        var model = new LoginModel(_db)
        {
            PageContext = new PageContext { HttpContext = httpContext }
        };

        var result = model.OnPost();
        
        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Index", redirect.PageName);
    }

    // [Fact]
    // public void OnPost_ReturnPageWithError_WhenInvalidCredentials()
    // {
    //     var passwordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("password123"));
        
    //     _db.Clientes.Add(new Cliente 
    //     { 
    //         Name = "Test", 
    //         Apellido = "User",
    //         Email = "test@test.com",
    //         Password = passwordHash,
    //         FechaRegistro = DateTime.Now 
    //     });
    //     _db.SaveChanges();
        
    //     var httpContext = new DefaultHttpContext();
    //     httpContext.Request.Form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
    //     {
    //         { "Email", "test@test.com" },
    //         { "Password", "wrongpassword" }
    //     });
    //     httpContext.Items["MS_HttpContext"] = httpContext;
        
    //     var model = new LoginModel(_db)
    //     {
    //         PageContext = new PageContext { HttpContext = httpContext }
    //     };

    //     var result = model.OnPost();
        
    //     Assert.IsType<PageResult>(result);
    //     Assert.True(model.ShowPassword);
    // }

    [Fact]
    public void OnPostVerificar_ReturnPage_WhenEmailNotInForm()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>());
        _model.PageContext = new PageContext { HttpContext = httpContext };
        
        var result = _model.OnPostVerificar();
        
        Assert.IsType<PageResult>(result);
    }
}

public class UsuarioModelTests : IDisposable
{
    private readonly AppDbContext _db;

    public UsuarioModelTests()
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
    public void OnGet_SetEmailPrefllenado_WhenEmailProvided()
    {
        var model = new UsuarioModel(_db);
        
        model.OnGet("test@test.com");

        Assert.Equal("test@test.com", model.EmailPrefllenado);
    }

    [Fact]
    public void OnGet_SetEmptyEmail_WhenNoEmailProvided()
    {
        var model = new UsuarioModel(_db);
        
        model.OnGet(null);

        Assert.Equal("", model.EmailPrefllenado);
    }

    [Fact]
    public async Task OnPost_RedirectToCuentas_WhenSuccess()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
        {
            { "Name", "Nuevo" },
            { "Apellido", "Usuario" },
            { "Email", "nuevo@test.com" },
            { "Password", "Password123!" }
        });
        httpContext.Session = new TestSession();
        
        var model = new UsuarioModel(_db)
        {
            PageContext = new PageContext { HttpContext = httpContext }
        };

        var result = await model.OnPost();
        
        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Cuentas", redirect.PageName);
        Assert.Single(_db.Clientes);
    }

    [Fact]
    public async Task OnPost_ReturnPage_WhenEmailAlreadyExists()
    {
        var passwordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("test123"));
        
        _db.Clientes.Add(new Cliente 
        { 
            Name = "Existente", 
            Apellido = "User",
            Email = "existente@test.com",
            Password = passwordHash,
            FechaRegistro = DateTime.Now 
        });
        _db.SaveChanges();
        
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
        {
            { "Name", "Nuevo" },
            { "Apellido", "Usuario" },
            { "Email", "existente@test.com" },
            { "Password", "password123" }
        });
        
        var model = new UsuarioModel(_db)
        {
            PageContext = new PageContext { HttpContext = httpContext }
        };

        var result = await model.OnPost();
        
        Assert.IsType<PageResult>(result);
        Assert.Single(_db.Clientes);
    }
}
