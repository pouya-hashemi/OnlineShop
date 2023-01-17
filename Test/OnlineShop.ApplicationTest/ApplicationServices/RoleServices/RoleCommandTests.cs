using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.ApplicationServices.RoleServices.Commands;
using OnlineShop.ApplicationTest.Fixtures;
using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;
using OnlineShop.TestShareContent.DataGenerators;

namespace OnlineShop.ApplicationTest.ApplicationServices.RoleServices;

[Collection("Database collection")]
public class RoleCommandTests : IAsyncLifetime
{
    private readonly IAppDbContext _context;
    private readonly Func<Task> _resetDatabase;
    private readonly EntityGenerator _entityGenerator;
    private readonly IRoleManager _roleManager;

    public RoleCommandTests(DatabaseFixture databaseFixture)
    {
        _context = databaseFixture.DbContext;
        _resetDatabase = databaseFixture.ResetDatabaseAsync;
        _entityGenerator = new EntityGenerator();
        _roleManager = new RoleManager(_context);
    }


    public static IEnumerable<object[]> CreateRole_ShouldAddToDatabase_WhenDataIsCorrect_Data()
    {
        yield return new object[]
        {
            new CreateRoleCommand()
            {
                Name = "Vendor",
            }
        };
    }

    [Theory]
    [MemberData(nameof(CreateRole_ShouldAddToDatabase_WhenDataIsCorrect_Data))]
    public async Task CreateRole_ShouldAddToDatabase_WhenDataIsCorrect(CreateRoleCommand command)
    {
        //Arrange
        var handler = new CreateRoleHandler(_roleManager, _context);
        //Act
        var roleDto = await handler.Handle(command, default);
        var existsInDatabase = await _context.Roles.AnyAsync(a => a.Id == roleDto.Id);
        //Assert
        Assert.True(existsInDatabase);
    }

    //********************************************************************//
    public static IEnumerable<object[]> UpdateRole_ShouldUpdateDatabase_WhenDataIsCorrect_Data()
    {
        yield return new object[]
        {
            new UpdateRoleCommand()
            {
                Name = "Vendor",
            }
        };
    }

    [Theory]
    [MemberData(nameof(UpdateRole_ShouldUpdateDatabase_WhenDataIsCorrect_Data))]
    public async Task UpdateRole_ShouldUpdateDatabase_WhenDataIsCorrect(UpdateRoleCommand command)
    {
        //Arrange
        var role = _entityGenerator.GenerateRole;
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
        command.RoleId = role.Id;
        var handler = new UpdateRoleHandler(_context, _roleManager);

        //Act
        await handler.Handle(command, default);
        var roleInDatabase = await _context.Roles.Where(w => w.Id == command.RoleId).FirstOrDefaultAsync();
        //Assert
        Assert.Equal(command.Name, roleInDatabase.Name);
    }

    //********************************************************************//

    [Fact]

    public async Task DeleteRole_ShouldDeleteDatabase_WhenDataIsCorrect()
    {
        //Arrange
        var command = new DeleteRoleCommand();
        var role = _entityGenerator.GenerateRole;
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
        command.RoleId = role.Id;
        var handler = new DeleteRoleHandler(_context,_roleManager);
        
        //Act
        await handler.Handle(command, default);
        var roelExists = await _context.Roles.Where(w => w.Id == command.RoleId).AnyAsync();
        //Assert
        Assert.False(roelExists);
    }

    //********************************************************************//
    
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync() => _resetDatabase();
}