using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Application.ApplicationServices.CartServices.Commands;
using OnlineShop.ApplicationTest.Fixtures;
using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;
using OnlineShop.TestShareContent.DataGenerators;


namespace OnlineShop.ApplicationTest.ApplicationServices.CartServices;
[Collection("Service collection")]
public class CartCommandTests: IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;
    private readonly IAppDbContext _context;
    private readonly ICartManager _cartManager;
    private readonly IProductManager _productManager;
    private readonly EntityGenerator _entityGenerator;
    private readonly FormFileGenerator _formFileGenerator;

    public CartCommandTests(ServiceFixture databaseFixture)
    {
        _resetDatabase = databaseFixture.ResetDatabaseAsync;
        _context = databaseFixture.DbContext;
        _cartManager = databaseFixture.ServiceProvider.GetService<ICartManager>();
        _entityGenerator = new EntityGenerator();
        _formFileGenerator = new FormFileGenerator();
        _productManager = databaseFixture.ServiceProvider.GetService<IProductManager>();
    }
    
    [Fact]
    public async Task CreateCartHandler_ShouldThrowNotFoundException_WhenProductIsNotAvailableInDb()
    {
        //Arrange
        var request = new CreateCartCommand()
        {
            Token = Guid.NewGuid().ToString(),
            Quantity = 10,
           ProductId = 1,
        };
        var handler = new CreateCartHandler(_context,_cartManager);
        //Act
        var act = async () => { await handler.Handle(request, default); };
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    //*******************************************//
    [Fact]
    public async Task CreateCartHandler_ShouldInsertCartInDb()
    {
        //Arrange
        var productInDb = _entityGenerator.GenerateProduct;
        _productManager.ChangeQuantity(productInDb,100);
        _context.Products.Add(productInDb);
        await _context.SaveChangesAsync();
        var request = new CreateCartCommand()
        {
            Token = Guid.NewGuid().ToString(),
            Quantity = 10,
           ProductId = productInDb.Id,
        };
        var handler = new CreateCartHandler(_context,_cartManager);
        //Act
        var cartDto= await handler.Handle(request, default); 
        //Assert
        var cartExists =await _context.Carts.AnyAsync(a => a.Id == cartDto.Id);
         Assert.True(cartExists);
    }

    //*******************************************//
    
    [Fact]
    public async Task DeleteCartHandler_ShouldThrowNotFoundException_WhenCartIsNotAvailableInDb()
    {
        //Arrange
        var request = new DeleteCartCommand()
        {
          CartId = 1,
        };
        var handler = new DeleteCartHandler(_context);
        //Act
        var act = async () => { await handler.Handle(request, default); };
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    //*******************************************//
    [Fact]
    public async Task DeleteCartHandler_ShouldDeleteCartFromDb()
    {
        //Arrange
        var cartInDb = _entityGenerator.GenerateCart;
        _context.Carts.Add(cartInDb);
        await _context.SaveChangesAsync();
        
        var request = new DeleteCartCommand()
        {
          CartId = cartInDb.Id,
        };
        var handler = new DeleteCartHandler(_context);
        //Act
         await handler.Handle(request, default);
        //Assert
        var cartExists = await _context.Carts.AnyAsync(a => a.Id == cartInDb.Id);
        Assert.False(cartExists);
    }

    //*******************************************//
    

    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await _resetDatabase();
        _formFileGenerator.Dispose();
    }
}