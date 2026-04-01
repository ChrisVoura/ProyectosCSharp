using MiPrimeraWebApp.Data;

namespace MiPrimeraWebApp.Tests;

public class ProductoModelTests
{
    private static Producto CreateProductoWithDefaults()
    {
        return new Producto
        {
            Name = "Test",
            Price = 10.00m,
            Category = "Cat",
            ImageUrl = "url"
        };
    }

    [Fact]
    public void Producto_RequiredFields_AreNullable()
    {
        var producto = new Producto
        {
            Name = "Test",
            Price = 10.00m,
            Category = "Cat",
            ImageUrl = "url"
        };

        Assert.Equal(0, producto.Id);
        Assert.Equal("Test", producto.Name);
        Assert.Equal(10.00m, producto.Price);
        Assert.Null(producto.Description);
        Assert.Equal("Cat", producto.Category);
        Assert.Equal("url", producto.ImageUrl);
    }

    [Fact]
    public void Producto_ValidData_SetCorrectly()
    {
        var producto = new Producto
        {
            Name = "Test Product",
            Price = 25.99m,
            Description = "Test Description",
            Category = "Test Category",
            ImageUrl = "https://example.com/image.jpg"
        };

        Assert.Equal("Test Product", producto.Name);
        Assert.Equal(25.99m, producto.Price);
        Assert.Equal("Test Description", producto.Description);
        Assert.Equal("Test Category", producto.Category);
        Assert.Equal("https://example.com/image.jpg", producto.ImageUrl);
    }

    [Fact]
    public void Producto_Description_IsOptional()
    {
        var producto = CreateProductoWithDefaults();

        Assert.Null(producto.Description);
    }

    [Fact]
    public void Producto_Price_CanBeZero()
    {
        var producto = CreateProductoWithDefaults();
        producto.Price = 0;

        Assert.Equal(0, producto.Price);
    }

    [Fact]
    public void Producto_ImageUrl_CanBeEmptyString()
    {
        var producto = CreateProductoWithDefaults();
        producto.ImageUrl = "";

        Assert.Equal(string.Empty, producto.ImageUrl);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(0.01)]
    [InlineData(99.99)]
    [InlineData(1000)]
    [InlineData(999999.99)]
    public void Producto_Price_AcceptsVariousValues(decimal price)
    {
        var producto = CreateProductoWithDefaults();
        producto.Price = price;

        Assert.Equal(price, producto.Price);
    }

    [Fact]
    public void Producto_Id_DefaultsToZero()
    {
        var producto = CreateProductoWithDefaults();

        Assert.Equal(0, producto.Id);
    }

    [Fact]
    public void Producto_MultipleImageUrls_CanBeCombined()
    {
        var producto = CreateProductoWithDefaults();
        producto.ImageUrl = "url1,url2,url3";

        var urls = producto.ImageUrl?.Split(',');
        Assert.NotNull(urls);
        Assert.Equal(3, urls.Length);
        Assert.Equal("url1", urls[0]);
        Assert.Equal("url2", urls[1]);
        Assert.Equal("url3", urls[2]);
    }
}
