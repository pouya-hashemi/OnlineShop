using OnlineShop.Domain.Entities;
using OnlineShop.Domain.EntityPropertyConfigurations;
using OnlineShop.Domain.Exceptions;
using OnlineShop.TestShareContent.DataGenerators;

namespace OnlineShop.DomainTest.EntitiesTests;

public class CartTests
{
    private readonly EntityGenerator _entityGenerator;

    public CartTests()
    {
        _entityGenerator = new EntityGenerator();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("11111-1111")]
    public void SetToken_ShouldThrowNullOrEmptyException_WhenTokenIsNotValid(string token)
    {
        //Arrange
        var cart = _entityGenerator.GenerateCart;
        //Act
        var act = () => { cart.SetToken(token); };
        //Assert
        Assert.Throws<NullOrEmptyException>(act);
    }

    [Fact]
    public void SetToken_ShouldChangeToken()
    {
        //Arrange
        var cart = _entityGenerator.GenerateCart;
        var token = Guid.NewGuid().ToString();
        //Act
        cart.SetToken(token);
        //Assert
        Assert.Equal(token, cart.Token.ToString());
    }

    [Theory]
    [InlineData(null)]
    public void SetProduct_ShouldThrowNullOrEmptyException_WhenProductIsNull(Product product)
    {
        //Arrange
        var cart = _entityGenerator.GenerateCart;
        //Act
        var act = () => { cart.SetProduct(product); };
        //Assert
        Assert.Throws<NullOrEmptyException>(act);
    }
    
    [Fact]
    public void SetProduct_ShouldChangeProductAndProductId()
    {
        //Arrange
        var cart = _entityGenerator.GenerateCart;
        var product = new Product("test", "image", 99, 45, _entityGenerator.GenerateCategory,"imageUrl");
        product.Id = 90;
        //Act
        cart.SetProduct(product);
        //Assert
        Assert.Equivalent(product,cart.Product);
        Assert.Equal(product.Id,cart.ProductId);
    }

    [Theory]
    [InlineData(CartPropertyConfigurations.MinQuantity - 1)]
    public void SetQuantity_ShouldThrowLessThanException_WhenQuantityIsNotValid(int quantity)
    {
        //Arrange
        var cart = _entityGenerator.GenerateCart;
        //Act
        var act = () => { cart.SetQuantity(quantity); };
        //Assert
        Assert.Throws<LessThanException>(act);
    }

    [Fact]
    public void SetQuantity_ShouldChangeQuantity()
    {
        //Arrange
        var cart = _entityGenerator.GenerateCart;
        //Act
        cart.SetQuantity(12);
        //Assert
        Assert.Equal(12,cart.Quantity);
    }
    
    [Theory]
    [InlineData(CartPropertyConfigurations.MinPrice - 1)]
    public void SetPrice_ShouldThrowLessThanException_WhenPriceIsNotValid(int price)
    {
        //Arrange
        var cart = _entityGenerator.GenerateCart;
        //Act
        var act = () => { cart.SetPrice(price); };
        //Assert
        Assert.Throws<LessThanException>(act);
    }
    [Fact]
    public void SetPrice_ShouldChangePrice()
    {
        //Arrange
        var cart = _entityGenerator.GenerateCart;
        //Act
        cart.SetPrice(99);
        //Assert
        Assert.Equal(99,cart.Price);
    }
}