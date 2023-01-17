using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;
using OnlineShop.DomainTest.Fixtures;
using OnlineShop.TestShareContent.DataGenerators;

namespace OnlineShop.DomainTest.DomainServiceTests;

[Collection("Database collection")]
public class ProductManagerTests:IAsyncLifetime
{
    private readonly IAppDbContext _context;
    private readonly Func<Task> _resetDatabase;
    private readonly EntityGenerator _entityGenerator;
    private readonly IProductManager _productManager;

    public ProductManagerTests(DatabaseFixture databaseFixture)
    {
        _context = databaseFixture.DbContext;
        _resetDatabase = databaseFixture.ResetDatabaseAsync;
        _entityGenerator = new EntityGenerator();
        _productManager = new ProductManager(_context);
    }

    public static IEnumerable<object[]>
        ChangeName_ShouldThrowAlreadyExistsException_WhenNameIsDuplicated_Data()
    {
        yield return new object[]
        {
            "product1"
        };
    }

    [Theory]
    [MemberData(nameof(ChangeName_ShouldThrowAlreadyExistsException_WhenNameIsDuplicated_Data))]
    public async Task ChangeName_ShouldThrowAlreadyExistsException_WhenNameIsDuplicated(string name)
    {
        //Arrange

        var productInDb = _entityGenerator.GenerateProduct;
        productInDb.SetName(name);
        _context.Products.Add(productInDb);
        await _context.SaveChangesAsync();

        var productToUpdate = _entityGenerator.GenerateProduct;

        //Act
        var act = async () => { await _productManager.ChangeNameAsync(productToUpdate, name); };

        //Assert
        await Assert.ThrowsAsync<AlreadyExistException>(act);
    }
    //********************************************************************//
    
    public static IEnumerable<object[]>
        ChangeName_ShouldChangeTheName_WhenNameIsCorrect_Data()
    {
        yield return new object[]
        {
            "product1"
        };
    }
    [Theory]
    [MemberData(nameof(ChangeName_ShouldChangeTheName_WhenNameIsCorrect_Data))]
    public async Task ChangeName_ShouldChangeTheName_WhenNameIsCorrect(string name)
    {
        //Arrange
        var product = _entityGenerator.GenerateProduct;

        //Act
        await _productManager.ChangeNameAsync(product, name);

        //Assert
        Assert.Equal(name,product.Name);
    }
    
    //********************************************************************//
    
    public static IEnumerable<object[]>
        CreateCategory_ShouldThrowAlreadyExistsException_WhenNameIsDuplicated_Data()
    {
        yield return new object[]
        {
            "product1"
        };
    }

    [Theory]
    [MemberData(nameof(CreateCategory_ShouldThrowAlreadyExistsException_WhenNameIsDuplicated_Data))]
    public async Task CreateCategory_ShouldThrowAlreadyExistsException_WhenNameIsDuplicated(string name)
    {
        //Arrange

        var productInDb = _entityGenerator.GenerateProduct;
        productInDb.SetName(name);
        _context.Products.Add(productInDb);
        await _context.SaveChangesAsync();

        var correctProduct = _entityGenerator.GenerateProduct;

        //Act
        var act = async () => { await _productManager.CreateProductAsync(name,correctProduct.ImageUrl,correctProduct.Price,correctProduct.Quantity,correctProduct.Category); };

        //Assert
        await Assert.ThrowsAsync<AlreadyExistException>(act);
    }
    //********************************************************************//
    
     public static IEnumerable<object[]>
        CreateCategory_ShouldReturnProduct_WhenDataIsCorrect_Data()
    {
        yield return new object[]
        {
            new EntityGenerator().GenerateProduct
        };
    }

    [Theory]
    [MemberData(nameof(CreateCategory_ShouldReturnProduct_WhenDataIsCorrect_Data))]
    public async Task CreateCategory_ShouldReturnProduct_WhenDataIsCorrect(Product product)
    {
        //Arrange

        //Act
       var productCreated=await _productManager.CreateProductAsync(product.Name,product.ImageUrl,product.Price,product.Quantity,product.Category); 

        //Assert
        Assert.Equivalent(product, productCreated);
    }
    //********************************************************************//
    
    
    
    

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}