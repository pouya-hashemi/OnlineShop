using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using OnlineShop.Application.ApplicationServices.UserServices.Commands;

using OnlineShop.ApplicationTest.Fixtures;
using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;
using OnlineShop.TestShareContent.DataGenerators;

namespace OnlineShop.ApplicationTest.ApplicationServices.UserServices;

[Collection("Database collection")]
public class UserCommandTests : IAsyncLifetime
{
    private readonly IAppDbContext _dbContext;
    private readonly Func<Task> _resetDatabase;
    private readonly IUserManager _userManager;
    private readonly EntityGenerator _entityGenerator;

    public UserCommandTests(DatabaseFixture databaseFixture)
    {
        _dbContext = databaseFixture.DbContext;
        _resetDatabase = databaseFixture.ResetDatabaseAsync;
        _userManager = new UserManager(_dbContext, new HashManager());
        _entityGenerator = new EntityGenerator();
    }

    public static IEnumerable<object[]> CreateUser_ShouldAddToDatabase_WhenDataIsCorrect_Data()
    {
        yield return new object[]
        {
            new CreateUserCommand()
            {
                Username = "pouyaHashemi",
                Password = "123456",
                PasswordReEnter = "123456",
                UserTitle = "Pouya Hashemi",
            }
        };
    }

    [Theory]
    [MemberData(nameof(CreateUser_ShouldAddToDatabase_WhenDataIsCorrect_Data))]
    public async Task CreateUser_ShouldAddToDatabase_WhenDataIsCorrect(CreateUserCommand command)
    {
        //Arrange
        var handler = new CreateUserHandler(_userManager, _dbContext);
        //Act
        var userDto = await handler.Handle(command, default);
        var existsInDatabase = await _dbContext.Users.AnyAsync(a => a.Id == userDto.Id);
        //Assert
        Assert.True(existsInDatabase);
    }

    //********************************************************************//
    public static IEnumerable<object[]> UpdateUser_ShouldUpdateData_WhenDataIsCorrect_Data()
    {
        yield return new object[]
        {
            new UpdateUserCommand()
            {
                Username = "pouyaHashemi",
                UserTitle = "Pouya Hashemi",
            }
        };
    }

    [Theory]
    [MemberData(nameof(UpdateUser_ShouldUpdateData_WhenDataIsCorrect_Data))]
    public async Task UpdateUser_ShouldUpdateData_WhenDataIsCorrect(UpdateUserCommand command)
    {
        //Arrange
        var user = _entityGenerator.GenerateUser;
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        command.UserId = user.Id;
        var handler = new UpdateUserHandler(_dbContext, _userManager);
        //Act
        await handler.Handle(command, default);
        //Assert
        var userAfterUpdate=await _dbContext.Users.FirstOrDefaultAsync(f => f.Id == user.Id);
        
        Assert.Equal(command.Username,userAfterUpdate.Username);
        Assert.Equal(command.UserTitle,userAfterUpdate.UserTitle);
    }
    //*************************************************************
    public static IEnumerable<object[]> UpdateUser_ShouldThrowNotFoundException_WhenUserDontExists_Data()
    {
        yield return new object[]
        {
            new UpdateUserCommand()
            {
                Username = "pouyaHashemi",
                UserTitle = "Pouya Hashemi",
            }
        };
    }

    [Theory]
    [MemberData(nameof(UpdateUser_ShouldThrowNotFoundException_WhenUserDontExists_Data))]
    public async Task UpdateUser_ShouldThrowNotFoundException_WhenUserDontExists(UpdateUserCommand command)
    {
        //Arrange
        var handler = new UpdateUserHandler(_dbContext, _userManager);
        //Act
        var act = async () =>
        {
            await handler.Handle(command, default);
        };
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }
    //*************************************************************

    [Fact]
    public async Task DeleteUser_ShouldThrowNotFoundException_WhenUserDontExists()
    {
        //Arrange
        var handler = new DeleteUserHandler(_dbContext, _userManager);
        //Act
        var act = async () =>
        {
            await handler.Handle(new DeleteUserCommand(), default);
        };
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }
    //*************************************************************
  [Fact]
    public async Task DeleteUser_ShouldDeleteData_WhenDataIsCorrect_Data( )
    {
        //Arrange
        var user = _entityGenerator.GenerateUser;
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        var command = new DeleteUserCommand()
        {
            UserId = user.Id
        };
        var handler = new DeleteUserHandler(_dbContext, _userManager);
        //Act
        await handler.Handle(command, default);
        //Assert
        var userExists=await _dbContext.Users.AnyAsync(f => f.Id == user.Id);
        
        Assert.False(userExists);
    }
    //*************************************************************

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}