using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable.Moq;
using Moq;
using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.EntityPropertyConfigurations;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;
using OnlineShop.DomainTest.Fixtures;
using OnlineShop.TestShareContent.DataGenerators;


namespace OnlineShop.DomainTest.DomainServiceTests;

[Collection("Service collection")]
public class UserManagerTests : IAsyncLifetime
{
    private readonly EntityGenerator _entityGenerator;
    private readonly IAppDbContext _context;
    private readonly Func<Task> _resetDatabase;
    private readonly IUserManager _userManager;

    private User _userInDb = default;


    public UserManagerTests(ServiceFixture serviceFixture)
    {
        _entityGenerator = new EntityGenerator();
        _context = serviceFixture.DbContext;
        _resetDatabase = serviceFixture.ResetDatabaseAsync;
        _userManager = serviceFixture.ServiceProvider.GetService<IUserManager>();
    }


    [Fact]
    public async Task ChangeUsername_ShouldThrowAlreadyExistException_WhenUsernameExists()
    {
        //Arrange
        var userInDb = _entityGenerator.GenerateUser;
        _context.Users.Add(userInDb);
        await _context.SaveChangesAsync();

        var userToUpdate = userInDb;
        userToUpdate.Id = 0;
        //Act
        
        var act = async () => { await _userManager.ChangeUsernameAsync(userToUpdate, userInDb.UserName); };
        
        //Assert
        await Assert.ThrowsAsync<AlreadyExistException>(act);
    }
    
    


    //*************************************************************//
    
    public static IEnumerable<object[]> ChangeUsername_ShouldThrowNotFound_WhenUserNotExists_Data()
    {
        yield return new object[]
        {
            null
        };
        
    }

    [Theory]
    [MemberData(nameof(ChangeUsername_ShouldThrowNotFound_WhenUserNotExists_Data))]
    public async Task ChangeUsername_ShouldThrowNotFound_WhenUserNotExists(User user)
    {
        //Arrange
        //Act

        var act =async () => {await _userManager.ChangeUsernameAsync(user, "asdasd"); };

        //Assert
       await Assert.ThrowsAsync<ArgumentNullException>(act);
    }


    //*************************************************************//
    public static IEnumerable<object[]> ChangeUserTitle_ShouldThrowNotFound_WhenUserNotExists_Data()
    {
        yield return new object[]
        {
            null
        };
        
    }

    [Theory]
    [MemberData(nameof(ChangeUserTitle_ShouldThrowNotFound_WhenUserNotExists_Data))]
    public async Task ChangeUserTitle_ShouldThrowNotFound_WhenUserNotExists(User user)
    {
        //Arrange
        //Act

        var act = () => { _userManager.ChangeUserTitle(user, "asdasd"); };

        //Assert
         Assert.Throws<ArgumentNullException>(act);
    }


    //*************************************************************//
    public static IEnumerable<object[]> ChangePassword_ShouldThrowNotFound_WhenUserNotExists_Data()
    {
        yield return new object[]
        {
            null
        };
        
    }
    [Theory]
    [MemberData(nameof(ChangePassword_ShouldThrowNotFound_WhenUserNotExists_Data))]
    public async Task  ChangePassword_ShouldThrowNotFound_WhenUserNotExists(User user)
    {
        //Arrange
        //Act

        var act =async () => {await _userManager.ChangePassword(user, "asdasd","asdasd","asdasd"); };

        //Assert
         await Assert.ThrowsAsync<ArgumentNullException>(act);
    }


    //*************************************************************//
    public static IEnumerable<object[]> PasswordChange_ShouldThrowBadRequest_WhenPasswordIsWrong_Data()
    {
        yield return new object[]
        {
            "123456",
            "999999",
            "999999",
        };

        yield return new object[]
        {
            "123123",
            "888888",
            "999999",
        };
    }

    [Theory]
    [MemberData(nameof(PasswordChange_ShouldThrowBadRequest_WhenPasswordIsWrong_Data))]
    public async Task PasswordChange_ShouldThrowBadRequest_WhenPasswordIsWrong(string password, string newPassword,
        string newPasswordRepeat)
    {
        //Arrange
        var user = _entityGenerator.GenerateUser;
        //Act

        var act =async () => {await _userManager.ChangePassword(user, password, newPassword, newPasswordRepeat); };

        //Assert
        await Assert.ThrowsAsync<BadRequestException>(act);
    }


    


   


    //*************************************************************//


    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync() => _resetDatabase();
}