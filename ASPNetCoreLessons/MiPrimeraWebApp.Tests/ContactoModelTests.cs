using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using MiPrimeraWebApp.Pages;

namespace MiPrimeraWebApp.Tests;

public class ContactoModelTests
{
    private ContactoModel CreateModelWithTempData()
    {
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        
        var model = new ContactoModel
        {
            Nombre = "Test",
            Email = "test@test.com",
            PageContext = new PageContext
            {
                HttpContext = httpContext
            }
        };
        model.TempData = tempData;
        
        return model;
    }

    [Fact]
    public void ContactoModel_RequiredPropertiesExist()
    {
        var model = new ContactoModel
        {
            Nombre = "Test",
            Email = "test@test.com"
        };

        Assert.NotNull(model.Nombre);
        Assert.NotNull(model.Email);
    }

    [Fact]
    public void OnPost_ReturnSuccessMessage_WhenModelStateIsValid()
    {
        var model = CreateModelWithTempData();
        model.Nombre = "Juan Pérez";
        model.Email = "juan@test.com";

        var result = model.OnPost();

        Assert.IsType<RedirectToPageResult>(result);
    }

    [Fact]
    public void OnGet_InitializeModel()
    {
        var model = new ContactoModel
        {
            Nombre = "Test",
            Email = "test@test.com"
        };

        model.OnGet();

        Assert.NotNull(model.Nombre);
        Assert.NotNull(model.Email);
    }

    [Theory]
    [InlineData("Juan", "juan@test.com")]
    [InlineData("María García", "maria.garcia@ejemplo.com")]
    [InlineData("Test User", "user123@domain.org")]
    public void OnPost_AcceptsVariousValidInputs(string nombre, string email)
    {
        var model = CreateModelWithTempData();
        model.Nombre = nombre;
        model.Email = email;

        var result = model.OnPost();

        Assert.IsType<RedirectToPageResult>(result);
    }
}
