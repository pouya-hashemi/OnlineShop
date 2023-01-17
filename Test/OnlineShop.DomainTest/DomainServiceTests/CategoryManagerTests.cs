
using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;
using OnlineShop.DomainTest.Fixtures;
using OnlineShop.TestShareContent.DataGenerators;

namespace OnlineShop.DomainTest.DomainServiceTests;

[Collection("Database collection")]
public class CategoryManagerTests : IAsyncLifetime
{
    private readonly IAppDbContext _context;
    private readonly EntityGenerator _entityGenerator;
    private readonly ICategoryManager _categoryManager;
    private Func<Task> _restDatabase;

    public CategoryManagerTests(DatabaseFixture databaseFixture)
    {
        _context = databaseFixture.DbContext;
        _restDatabase = databaseFixture.ResetDatabaseAsync;
        _entityGenerator = new EntityGenerator();
        _categoryManager = new CategoryManager(_context);
    }


    public static IEnumerable<object[]> ChangeName_ShouldThrowArgumentNullException_WhenInputIsNull_Data()
    {
        yield return new object[]
        {
            new EntityGenerator().GenerateCategory,
            null
        };
        yield return new object[]
        {
            null,
            "asd"
        };
        yield return new object[]
        {
            null,
            null
        };
    }

    [Theory]
    [MemberData(nameof(ChangeName_ShouldThrowArgumentNullException_WhenInputIsNull_Data))]
    public async Task ChangeName_ShouldThrowArgumentNullException_WhenInputIsNull(Category category, string name)
    {
        //Arrange

        //Act
        var act = async () => { await _categoryManager.ChangeNameAsync(category, name); };
        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(act);
    }

    //******************************************************************

    public static IEnumerable<object[]> ChangeName_ShouldThrowAlreadyExistException_WhenNameIsDuplicate_Data()
    {
        yield return new object[]
        {
            "Category"
        };
    }

    [Theory]
    [MemberData(nameof(ChangeName_ShouldThrowAlreadyExistException_WhenNameIsDuplicate_Data))]
    public async Task ChangeName_ShouldThrowAlreadyExistException_WhenNameIsDuplicate(string name)
    {
        //Arrange
        var categoryInDb = _entityGenerator.GenerateCategory;
        categoryInDb.SetName(name);
        _context.Categories.Add(categoryInDb);
        await _context.SaveChangesAsync();

        var categoryToUpdate = _entityGenerator.GenerateCategory;
        //Act
        var act = async () => { await _categoryManager.ChangeNameAsync(categoryToUpdate, name); };
        //Assert
        await Assert.ThrowsAsync<AlreadyExistException>(act);
    }
    //******************************************************************

    public static IEnumerable<object[]> ChangeName_ShouldUpdateName_WhenInputIsCorrect_Data()
    {
        yield return new object[]
        {
            "Category"
        };
    }

    [Theory]
    [MemberData(nameof(ChangeName_ShouldUpdateName_WhenInputIsCorrect_Data))]
    public async Task ChangeName_ShouldUpdateName_WhenInputIsCorrect(string name)
    {
        //Arrange

        var category = _entityGenerator.GenerateCategory;
        //Act
        await _categoryManager.ChangeNameAsync(category, name);
        //Assert
        Assert.Equal(name, category.Name);
    }
    
    //******************************************************************

    public static IEnumerable<object[]> CreateCategory_ShouldThrowAlreadyExists_WhenNameExistsInDb_Data()
    {
        yield return new object[]
        {
            "Category"
        };
    }

    [Theory]
    [MemberData(nameof(CreateCategory_ShouldThrowAlreadyExists_WhenNameExistsInDb_Data))]
    public async Task CreateCategory_ShouldThrowAlreadyExists_WhenNameExistsInDb(string name)
    {
        //Arrange
        
        var categoryInDb = _entityGenerator.GenerateCategory;
        categoryInDb.SetName(name);
        _context.Categories.Add(categoryInDb);
        await _context.SaveChangesAsync();
        
        //Act
        var act = async () => { await _categoryManager.CreateCategoryAsync(name); };
        //Assert
        await Assert.ThrowsAsync<AlreadyExistException>(act);
    }
//******************************************************************

    public static IEnumerable<object[]> CreateCategory_ShouldReturnCategory_WhenInputDataIsCorrect_Data()
    {
        yield return new object[]
        {
            "Category"
        };
    }

    [Theory]
    [MemberData(nameof(CreateCategory_ShouldReturnCategory_WhenInputDataIsCorrect_Data))]
    public async Task CreateCategory_ShouldReturnCategory_WhenInputDataIsCorrect(string name)
    {
        //Arrange
        var expected = new Category(name);
        //Act
        var category= await _categoryManager.CreateCategoryAsync(name);
        //Assert
        Assert.Equivalent(expected,category);
        // category.Should().BeEquivalentTo(expected);
    }


    public Task InitializeAsync() => Task.CompletedTask;


    public async Task DisposeAsync()
    {
        await _restDatabase();
    }
}