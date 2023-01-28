using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;
using OnlineShop.DomainTest.Fixtures;
using OnlineShop.TestShareContent.DataGenerators;

namespace OnlineShop.DomainTest.DomainServiceTests;

[Collection("Database collection")]
public class CartManagerTests : IAsyncLifetime
{
    private readonly ICartManager _cartManager;
    private readonly Func<Task> _resetDatabase;
    private readonly EntityGenerator _entityGenerator;
    private readonly IAppDbContext _context;

    public CartManagerTests(DatabaseFixture databaseFixture)
    {
        _cartManager = new CartManager(databaseFixture.DbContext, new StockManager(databaseFixture.DbContext));
        _resetDatabase = databaseFixture.ResetDatabaseAsync;
        _entityGenerator = new EntityGenerator();
        _context = databaseFixture.DbContext;
    }

    [Fact]
    public async Task CreateCartAsync_ShouldThrowArgumentNullException_WhenProductIsNull()
    {
        //Arrange

        //Act
        var act = async () => { await _cartManager.CreateCartAsync(Guid.NewGuid().ToString(), null, 1, default); };
        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(act);
    }
    [Theory]
    [InlineData(10,15)]
    [InlineData(10,11)]
    public async Task CreateCartAsync_ShouldThrowNotEnoughStockException_WhenProductHasNoStock(int stock,int cartQuantity)
    {
        //Arrange
        var productInDb = _entityGenerator.GenerateProduct;
        productInDb.SetQuantity(stock);
        _context.Products.Add(productInDb);
        await _context.SaveChangesAsync();
        //Act
        var act = async () => { await _cartManager.CreateCartAsync(Guid.NewGuid().ToString(), productInDb, cartQuantity,  default); };
        //Assert
        await Assert.ThrowsAsync<NotEnoughStockException>(act);
    }
    [Fact]
    public async Task CreateCartAsync_ShouldReturnValidCart()
    {
        //Arrange
        var stock = 10;
        var token = Guid.NewGuid().ToString();
        var quantity = 5;
        var productInDb = _entityGenerator.GenerateProduct;
        productInDb.SetQuantity(stock);
        _context.Products.Add(productInDb);
        await _context.SaveChangesAsync();

        var expectedCart = new Cart(token, productInDb, quantity, productInDb.Price);
        //Act
        var cart=await _cartManager.CreateCartAsync(token, productInDb, quantity,  default); 
        //Assert

         Assert.Equivalent(expectedCart,cart);
    }


    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await _resetDatabase();
    }
}