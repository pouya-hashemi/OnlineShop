
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.ApplicationServices.CategoryServices.Commands;
using OnlineShop.ApplicationTest.Fixtures;
using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;
using OnlineShop.TestShareContent.DataGenerators;

namespace OnlineShop.ApplicationTest.ApplicationServices.CategoryServices;
[Collection("Database collection")]
public class CategoryCommandTests:IAsyncLifetime
{
    private readonly IAppDbContext _context;
    private readonly EntityGenerator _entityGenerator;
    private readonly ICategoryManager _categoryManager;
    private Func<Task> _resetDatabase;


    public CategoryCommandTests(DatabaseFixture databaseFixture)
    {
        _context = databaseFixture.DbContext;
        _entityGenerator = new EntityGenerator();
        _categoryManager = new CategoryManager(_context);
        _resetDatabase = databaseFixture.ResetDatabaseAsync;
    }

    public static IEnumerable<object[]> CreateCategoryHandle_ShouldInsertCategoryInDatabase_Data()
    {
        yield return new object[]
        {
            new CreateCategoryCommand()
            {
                CategoryName = "Category"
            }
        };
    }

    [Theory]
    [MemberData(nameof(CreateCategoryHandle_ShouldInsertCategoryInDatabase_Data))]
    public async Task CreateCategoryHandle_ShouldInsertCategoryInDatabase(CreateCategoryCommand request)
    {
        //Arrange
        //Act
        var handler = new CreateCategoryHandler(_context, _categoryManager);
        var categoryDto = await handler.Handle(request, default);
        //Assert
        var categoryExists = await _context.Categories.AnyAsync(a => a.Id == categoryDto.Id);

        Assert.True(categoryExists);
    }
    
    //****************************************************************
    
    public static IEnumerable<object[]> UpdateCategoryHandle_ShouldUpdateCategoryNameInDatabase_Data()
    {
        yield return new object[]
        {
            new UpdateCategoryCommand()
            {
                CategoryName = "Category"
            }
        };
    }

    [Theory]
    [MemberData(nameof(UpdateCategoryHandle_ShouldUpdateCategoryNameInDatabase_Data))]
    public async Task UpdateCategoryHandle_ShouldUpdateCategoryNameInDatabase(UpdateCategoryCommand request)
    {
        //Arrange
        var categoryInDb = _entityGenerator.GenerateCategory;
        _context.Categories.Add(categoryInDb);
        await _context.SaveChangesAsync();
        
        request.CategoryId = categoryInDb.Id;
        //Act
        var handler = new UpdateCategoryHandler(_context, _categoryManager);
         await handler.Handle(request, default);
        //Assert
        var category = await _context.Categories.FirstOrDefaultAsync(a => a.Id == categoryInDb.Id);

        Assert.Equal(request.CategoryName,category.Name);
    }

    [Fact]
    public async Task DeleteCategoryHandle_ShouldRemoveDataFromDatabase()
    {
        //Arrange
        var categoryInDb = _entityGenerator.GenerateCategory;
        _context.Categories.Add(categoryInDb);
        await _context.SaveChangesAsync();
        
        //Act
        var handler = new DeleteCategoryHandler(_context, _categoryManager);
        await handler.Handle(new DeleteCategoryCommand()
        {
            CategoryId = categoryInDb.Id
        }, default);
        //Assert
        var categoryExists = await _context.Categories.AnyAsync(a => a.Id == categoryInDb.Id);

        Assert.False(categoryExists);
    }


    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await _resetDatabase();
    }
}