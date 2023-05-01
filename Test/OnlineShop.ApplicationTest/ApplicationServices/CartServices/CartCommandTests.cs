using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineShop.Application.ApplicationServices.CartServices.Commands;
using OnlineShop.ApplicationTest.Fixtures;
using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;
using OnlineShop.TestShareContent.Common;
using OnlineShop.TestShareContent.DataGenerators;
using OnlineShop.TestShareContent.SharedFixtures;

namespace OnlineShop.ApplicationTest.ApplicationServices.CartServices;
[Collection("Database collection")]
public class CartCommandTests: IAsyncLifetime, IClassFixture<FileServiceFixture>
{
    private readonly Func<Task> _resetDatabase;
    private readonly IAppDbContext _context;
    private readonly ICartManager _cartManager;
    private readonly IProductManager _productManager;
    private readonly IFileService _fileService;
    private readonly EntityGenerator _entityGenerator;
    private readonly FormFileGenerator _formFileGenerator;
    private readonly IConfiguration _configuration;
    public CartCommandTests(DatabaseFixture databaseFixture,
        FileServiceFixture fileServiceFixture)
    {
        _resetDatabase = databaseFixture.ResetDatabaseAsync;
        _context = databaseFixture.DbContext;
        _cartManager = new CartManager(_context, new StockManager(_context));
        _entityGenerator = new EntityGenerator();
        _fileService = fileServiceFixture.FileService;
        _formFileGenerator = new FormFileGenerator();
        _configuration = new Utilities().Configuration;
        _productManager = new ProductManager(_context, _fileService);
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