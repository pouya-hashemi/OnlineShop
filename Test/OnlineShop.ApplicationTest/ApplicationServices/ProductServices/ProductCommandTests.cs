using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineShop.Application.ApplicationServices.ProductServices.Commands;
using OnlineShop.ApplicationTest.Fixtures;
using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.EntityPropertyConfigurations;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;
using OnlineShop.TestShareContent.Common;
using OnlineShop.TestShareContent.DataGenerators;
using FileServiceFixture = OnlineShop.TestShareContent.SharedFixtures.FileServiceFixture;

namespace OnlineShop.ApplicationTest.ApplicationServices.ProductServices;

[Collection("Database collection")]
public class ProductCommandTests : IAsyncLifetime, IClassFixture<FileServiceFixture>
{
    private readonly Func<Task> _resetDatabase;
    private readonly IAppDbContext _context;
    private readonly IProductManager _productManager;
    private readonly IFileService _fileService;
    private readonly EntityGenerator _entityGenerator;
    private readonly FormFileGenerator _formFileGenerator;
    private readonly IConfiguration _configuration;

    public ProductCommandTests(DatabaseFixture databaseFixture,
        FileServiceFixture fileServiceFixture)
    {
        _resetDatabase = databaseFixture.ResetDatabaseAsync;
        _context = databaseFixture.DbContext;
        _productManager = new ProductManager(_context);
        _entityGenerator = new EntityGenerator();
        _fileService = fileServiceFixture.FileService;
        _formFileGenerator = new FormFileGenerator();
        _configuration = new Utilities().Configuration;
    }

    [Fact]
    public async Task CreateProductHandler_ShouldThrowNotFoundException_WhenCategoryIsNotAvailableInDb()
    {
        //Arrange
        var productSample = _entityGenerator.GenerateProduct;


        var request = new CreateProductCommand()
        {
            Price = productSample.Price,
            Quantity = productSample.Quantity,
            CategoryId = 1,
            ImageFile = _formFileGenerator.CreateImageFormFileJpg(),
            ProductName = productSample.Name
        };
        var handler = new CreateProductHandler(_context, _productManager, _fileService);
        //Act
        var act = async () => { await handler.Handle(request, default); };
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    //*******************************************//
    public static IEnumerable<object[]> CreateProductHandler_ShouldInsertToDatabase_WhenInputDataIsCorrect_Data()
    {
        yield return new object[]
        {
            new CreateProductCommand()
            {
                Price = ProductPropertyConfiguration.PriceMinValue,
                Quantity = ProductPropertyConfiguration.QuantityMinValue,
                ProductName = new string(Enumerable.Repeat('a', ProductPropertyConfiguration.NameMinLength).ToArray()),
            }
        };
        yield return new object[]
        {
            new CreateProductCommand()
            {
                Price = ProductPropertyConfiguration.PriceMinValue,
                Quantity = ProductPropertyConfiguration.QuantityMinValue,
                ProductName = new string(Enumerable.Repeat('a', ProductPropertyConfiguration.NameMaxLength).ToArray()),
            }
        };
    }

    [Theory]
    [MemberData(nameof(CreateProductHandler_ShouldInsertToDatabase_WhenInputDataIsCorrect_Data))]
    public async Task CreateProductHandler_ShouldInsertToDatabase_WhenInputDataIsCorrect(CreateProductCommand command)
    {
        //Arrange
        var category =
            new Category(new string(Enumerable.Repeat('a', CategoryPropertyConfiguration.NameMaxLength).ToArray()));
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        var handler = new CreateProductHandler(_context, _productManager, _fileService);
        command.CategoryId = category.Id;
        command.ImageFile = _formFileGenerator.CreateImageFormFileJpg();
        //Act
        var product = await handler.Handle(command, default);
        //Assert
        var productExists = await _context.Products.AnyAsync(a => a.Id == product.Id);
        Assert.True(productExists);
    }

    //******************************************//
    [Fact]
    public async Task UpdateProductHandler_ShouldThrowNotFoundException_WhenProductISNotAvailable()
    {
        //Arrange
        var handler = new UpdateProductHandler(_context, _productManager);
        var command = new UpdateProductCommand()
        {
            ProductId = 1,
            ProductName = "product 1",
            CategoryId = 1,
        };
        //Act
        var act = async () => { await handler.Handle(command, default); };
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }

//******************************************//
    [Fact]
    public async Task UpdateProductHandler_ShouldThrowNotFoundException_WhenCategoryIsNotAvailable()
    {
        //Arrange
        var product = _entityGenerator.GenerateProduct;
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var handler = new UpdateProductHandler(_context, _productManager);
        var command = new UpdateProductCommand()
        {
            ProductId = product.Id,
            ProductName = "product 1",
            CategoryId = product.CategoryId + 1
        };
        //Act
        var act = async () => { await handler.Handle(command, default); };
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }

//***********************************************//
    public static IEnumerable<object[]> UpdateProductHandler_ShouldUpdateDatabase_WhenInputDataIsCorrect_Data()
    {
        yield return new object[]
        {
            new UpdateProductCommand()
            {
                ProductName = new EntityGenerator().GenerateProduct.Name,
                Price = ProductPropertyConfiguration.PriceMinValue+5,
                Quantity = ProductPropertyConfiguration.QuantityMinValue+5
                
            }
        };
    }

    [Theory]
    [MemberData(nameof(UpdateProductHandler_ShouldUpdateDatabase_WhenInputDataIsCorrect_Data))]
    public async Task UpdateProductHandler_ShouldUpdateDatabase_WhenInputDataIsCorrect(UpdateProductCommand command)
    {
        //Arrange
        var productInDb = _entityGenerator.GenerateProduct;
        _context.Products.Add(productInDb);

        var categoryInDb = _entityGenerator.GenerateCategory;
        _context.Categories.Add(categoryInDb);

        await _context.SaveChangesAsync();

        command.ProductId = productInDb.Id;
        command.CategoryId = categoryInDb.Id;

        var handler = new UpdateProductHandler(_context, _productManager);

        //Act
        await handler.Handle(command, default);
        var productAfterUpdate = await _context.Products
            .Include(i => i.Category)
            .Where(w => w.Id == command.ProductId)
            .FirstOrDefaultAsync();
        //Assert

        Assert.Equal(command.CategoryId, productAfterUpdate.CategoryId);
        Assert.Equal(command.ProductName, productAfterUpdate.Name);
        Assert.Equal(command.Price, productAfterUpdate.Price);
        Assert.Equal(command.Quantity, productAfterUpdate.Quantity);
    }

    //*****************************************************************************
    [Fact]
    public async Task DeleteProductHandler_ShouldDeleteFromDb()
    {
        //Arrange
        var productInDb = _entityGenerator.GenerateProduct;
        _context.Products.Add(productInDb);
        await _context.SaveChangesAsync();
        
        var handler = new DeleteProductHandler(_context, _productManager);
        
        //Act
        await handler.Handle(new DeleteProductCommand() {ProductId = productInDb.Id}, default);
        
        var productExist = await _context.Products
            .Where(w => w.Id == productInDb.Id)
            .AnyAsync();
        
        //Assert
        Assert.False(productExist);



    }
    //*******************************************************
    [Fact]
    public async Task DeleteProductHandler_ShouldThrowNotFoundException_WhenProductIsNotAvailableInDb()
    {
        //Arrange
        var handler = new DeleteProductHandler(_context, _productManager);
        
        //Act
        var act=async()=> await handler.Handle(new DeleteProductCommand() {ProductId = 1}, default);
        
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }
    
    //******************************************************
    [Fact]
    public async Task ChangeProductImageHandler_ShouldThrowNotFoundException_WhenProductIsNotAvailable()
    {
        //Arrange
        var handler = new ChangeProductImageHandler(_context, _productManager,_fileService);
        
        //Act
        var act=async()=> await handler.Handle(new ChangeProductImageCommand() {ProductId = 1}, default);
        
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }
    
    //**********************************************//
    [Fact]
    public async Task ChangeProductImageHandler_ShouldChangeImage_WhenInputIsCorrect()
    {
        //Arrange
        var productInDb = _entityGenerator.GenerateProduct;
        _context.Products.Add(productInDb);
        await _context.SaveChangesAsync();
        var handler = new ChangeProductImageHandler(_context, _productManager,_fileService);
        var command = new ChangeProductImageCommand()
        {
            ProductId = productInDb.Id,
            ImageFile = _formFileGenerator.CreateImageFormFileJpg()
        };
        //Act
         await handler.Handle(command, default);
        var productAfterUpdate = await _context.Products
            .Where(w => w.Id == productInDb.Id)
            .FirstOrDefaultAsync();
        //Assert
        var exists=File.Exists(_configuration["FileStoringBasePath"] + productAfterUpdate.ImageUrl);
        Assert.True(exists);
    }
    

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await _resetDatabase();
        _formFileGenerator.Dispose();
    }
}