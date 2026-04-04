using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;
using MiPrimeraWebApp.Data;

namespace MiPrimeraWebApp.Tests;

public class ClienteModelTests
{
    [Fact]
    public void Cliente_Name_IsRequiredMember()
    {
        var property = typeof(Cliente).GetProperty("Name");
        Assert.NotNull(property);
        
        var requiredAttr = property!.GetCustomAttribute<RequiredMemberAttribute>();
        Assert.NotNull(requiredAttr);
    }

    [Fact]
    public void Cliente_Email_IsRequiredMember()
    {
        var property = typeof(Cliente).GetProperty("Email");
        Assert.NotNull(property);
        
        var requiredAttr = property!.GetCustomAttribute<RequiredMemberAttribute>();
        Assert.NotNull(requiredAttr);
    }

    [Fact]
    public void Cliente_FechaRegistro_IsRequiredMember()
    {
        var property = typeof(Cliente).GetProperty("FechaRegistro");
        Assert.NotNull(property);
        
        var requiredAttr = property!.GetCustomAttribute<RequiredMemberAttribute>();
        Assert.NotNull(requiredAttr);
    }

    [Fact]
    public void Cliente_HasEmailAddressAttribute()
    {
        var property = typeof(Cliente).GetProperty("Email");
        Assert.NotNull(property);
        
        var emailAttr = property!.GetCustomAttribute<EmailAddressAttribute>();
        Assert.NotNull(emailAttr);
    }

    [Fact]
    public void Cliente_EmailHasStringLength100()
    {
        var property = typeof(Cliente).GetProperty("Email");
        Assert.NotNull(property);
        
        var stringLengthAttr = property!.GetCustomAttribute<StringLengthAttribute>();
        Assert.NotNull(stringLengthAttr);
        Assert.Equal(100, stringLengthAttr!.MaximumLength);
    }

    [Fact]
    public void Cliente_FechaRegistro_HasDefaultValue()
    {
        var before = DateTime.Now.AddSeconds(-1);
        
        var cliente = new Cliente
        {
            Name = "Test",
            Apellido = "Test",
            Email = "test@test.com",
            Password = "test123",
            FechaRegistro = DateTime.Now
        };
        
        var after = DateTime.Now.AddSeconds(1);

        Assert.InRange(cliente.FechaRegistro, before, after);
    }

    [Fact]
    public void Cliente_ValidData_PassesValidation()
    {
        var cliente = new Cliente
        {
            Name = "Test User",
            Apellido = "User",
            Email = "test@example.com",
            Password = "test123",
            FechaRegistro = DateTime.Now
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(cliente);
        var isValid = Validator.TryValidateObject(cliente, validationContext, validationResults, true);

        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Fact]
    public void Cliente_InvalidEmail_FailsValidation()
    {
        var cliente = new Cliente
        {
            Name = "Test User",
            Apellido = "User",
            Email = "not-an-email",
            Password = "test123",
            FechaRegistro = DateTime.Now
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(cliente);
        var isValid = Validator.TryValidateObject(cliente, validationContext, validationResults, true);

        Assert.False(isValid);
        Assert.Contains(validationResults, r => r.MemberNames.Contains("Email"));
    }

    [Fact]
    public void Cliente_NameHasNoStringLengthConstraint()
    {
        var property = typeof(Cliente).GetProperty("Name");
        Assert.NotNull(property);
        
        var stringLengthAttr = property!.GetCustomAttribute<StringLengthAttribute>();
        Assert.Null(stringLengthAttr);
    }

    [Fact]
    public void Cliente_Id_PropertyExists()
    {
        var property = typeof(Cliente).GetProperty("Id");
        Assert.NotNull(property);
        Assert.Equal(typeof(int), property!.PropertyType);
    }
}
