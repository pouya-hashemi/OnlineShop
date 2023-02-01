using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.EntityPropertyConfigurations;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;
using OnlineShop.DomainTest.Fixtures;
using OnlineShop.TestShareContent.DataGenerators;


namespace OnlineShop.DomainTest.DomainServiceTests;

[Collection("Service collection")]
public class RoleManagerTests:IAsyncLifetime
{
    private readonly EntityGenerator _entityGenerator;
    private readonly IAppDbContext _context;
    private readonly Func<Task> _resetDatabase;
    private readonly IRoleManager _roleManager;

    public RoleManagerTests(ServiceFixture serviceFixture)
    {
        _entityGenerator = new EntityGenerator();
        _context = serviceFixture.DbContext;
        _resetDatabase = serviceFixture.ResetDatabaseAsync;
        _roleManager = serviceFixture.ServiceProvider.GetService<IRoleManager>();
    }

    public static IEnumerable<object[]> ChangeName_ShouldThrowAlreadyExistsException_WhenNameExistsInDatabase_Data()
    {
        yield return new object[]
        {
            new Role(new string(Enumerable.Repeat('a', RolePropertyConfiguration.NameMinLength).ToArray())),
            new string(Enumerable.Repeat('b',RolePropertyConfiguration.NameMinLength).ToArray())
        };
    }
[Theory]
[MemberData(nameof(ChangeName_ShouldThrowAlreadyExistsException_WhenNameExistsInDatabase_Data))]
    public async Task ChangeName_ShouldThrowAlreadyExistsException_WhenNameExistsInDatabase(Role role, string name)
    {
        //Arrange
        var roleInDb = _entityGenerator.GenerateRole;
        roleInDb.SetName(name);
        _context.Roles.Add(roleInDb);
        await _context.SaveChangesAsync();
        //act
        var act = async () => { await _roleManager.ChangeNameAsync(role, name); };
        //Assert
        await Assert.ThrowsAsync<AlreadyExistException>(act);
    }
    //***************************************************************//
    
    public static IEnumerable<object[]> ChangeName_ShouldThrowArgumentNullException_WhenRoleIsNull_Data()
    {
        yield return new object[]
        {
            null,
            new string(Enumerable.Repeat('b',RolePropertyConfiguration.NameMinLength).ToArray())
        };
    }
    [Theory]
    [MemberData(nameof(ChangeName_ShouldThrowArgumentNullException_WhenRoleIsNull_Data))]
    public async Task ChangeName_ShouldThrowArgumentNullException_WhenRoleIsNull(Role role, string name)
    {
        //Arrange
        //act
        var act = async () => { await _roleManager.ChangeNameAsync(role, name); };
        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(act);
    }
    //***************************************************************//
    
    
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync() => _resetDatabase();
}