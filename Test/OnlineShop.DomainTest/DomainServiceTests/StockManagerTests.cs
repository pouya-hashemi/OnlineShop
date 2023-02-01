using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;
using OnlineShop.DomainTest.Fixtures;
using OnlineShop.TestShareContent.DataGenerators;

namespace OnlineShop.DomainTest.DomainServiceTests;

[Collection("Service collection")]
public class StockManagerTests : IAsyncLifetime
{
    private readonly IStockManager _stockManager;
    private readonly EntityGenerator _entityGenerator;
    private readonly Func<Task> _resetDatabase;
    private readonly IAppDbContext _context;

    public StockManagerTests(ServiceFixture serviceFixture)
    {
        _context = serviceFixture.DbContext;
        _stockManager = serviceFixture.ServiceProvider.GetService<IStockManager>();
        _resetDatabase = serviceFixture.ResetDatabaseAsync;
        _entityGenerator = new EntityGenerator();
    }

    [Fact]
    public async Task ProductHasSellableStockQuantityAsync_ShouldThrowNotFoundException_WhenProductIdDoesntExistsInDb()
    {
        //Arrange

        //Act
        var act = async () => { await _stockManager.ProductHasSellableStockQuantityAsync(1, 0); };
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    [Theory]
    [InlineData(10, 5, true)]
    [InlineData(10, 11, false)]
    [InlineData(10, 10, true)]
    public async Task ProductHasSellableStockQuantityAsync_ShouldReturnCorrectStockStatus(int quantity,
        int sellQuantity, bool expected)
    {
        //Arrange
        var productInDb = _entityGenerator.GenerateProduct;
        productInDb.SetQuantity(quantity);
        _context.Products.Add(productInDb);
        await _context.SaveChangesAsync();
        //Act
        var result = await _stockManager.ProductHasSellableStockQuantityAsync(productInDb.Id, sellQuantity);
        //Assert
        Assert.Equal(expected,result);
    }


    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await _resetDatabase();
    }
}