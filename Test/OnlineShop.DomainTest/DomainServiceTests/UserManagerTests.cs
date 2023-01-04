﻿using MockQueryable.Moq;
using Moq;
using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.EntityPropertyConfigurations;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;
using OnlineShop.DomainTest.Common;
using OnlineShop.DomainTest.Fixtures;

namespace OnlineShop.DomainTest.DomainServiceTests;

[Collection("Database collection")]
public class UserManagerTests : IAsyncLifetime
{
    private readonly EntityGenerator _entityGenerator;
    private readonly IAppDbContext _context;
    private readonly Func<Task> _resetDatabase;
    private readonly IUserManager _userManager;
    private readonly HashManager _hashManager;
    private User _userInDb = default;

    public UserManagerTests(DatabaseFixture databaseFixture)
    {
        _entityGenerator = new EntityGenerator();
        _context = databaseFixture.DbContext;
        _resetDatabase = databaseFixture.ResetDatabaseAsync;
        _hashManager = new HashManager();
        _userManager = new UserManager(_context, _hashManager);
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
        
        var act = async () => { await _userManager.ChangeUsernameAsync(userToUpdate, userInDb.Username); };
        
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
    public void  ChangePassword_ShouldThrowNotFound_WhenUserNotExists(User user)
    {
        //Arrange
        //Act

        var act = () => { _userManager.ChangePassword(user, "asdasd","asdasd","asdasd"); };

        //Assert
         Assert.Throws<ArgumentNullException>(act);
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

        var act = () => { _userManager.ChangePassword(user, password, newPassword, newPasswordRepeat); };

        //Assert
        Assert.Throws<BadRequestException>(act);
    }


    //*************************************************************//
    public static IEnumerable<object[]>
        PasswordChange_ShouldLengthException_WhenPasswordIsEmptyOrNullOrLengthNotOK_Data()
    {
        yield return new object[]
        {
            "123456",
            "",
            "",
            typeof(NullOrEmptyException)
        };

        yield return new object[]
        {
            "123123",
            null,
            null,
            typeof(NullOrEmptyException)
        };
        yield return new object[]
        {
            "123123",
            " ",
            " ",
            typeof(NullOrEmptyException)
        };
        yield return new object[]
        {
            "123123",
            new string(Enumerable.Repeat('a', UserPropertyConfiguration.PasswordMaxLength + 1).ToArray()),
            new string(Enumerable.Repeat('a', UserPropertyConfiguration.PasswordMaxLength + 1).ToArray()),
            typeof(MaxLengthException)
        };
        yield return new object[]
        {
            "123123",
            new string(Enumerable.Repeat('a', UserPropertyConfiguration.PasswordMinLength - 1).ToArray()),
            new string(Enumerable.Repeat('a', UserPropertyConfiguration.PasswordMinLength - 1).ToArray()),
            typeof(MinLengthException)
        };
    }

    [Theory]
    [MemberData(nameof(PasswordChange_ShouldLengthException_WhenPasswordIsEmptyOrNullOrLengthNotOK_Data))]
    public void PasswordChange_ShouldLengthException_WhenPasswordIsEmptyOrNullOrLengthNotOK(string password,
        string newPassword, string newPasswordRepeat, Type exceptionType)
    {
        //Arrange
      
        var user = _entityGenerator.GenerateUser;
        user.SetPassword(_hashManager.CreateHash("123123"));
        //Act

        var act = () => { _userManager.ChangePassword(user, password, newPassword, newPasswordRepeat); };

        //Assert
        Assert.Throws(exceptionType, act);
    }


    //*************************************************************//

    public static IEnumerable<object[]> PasswordChange_ShouldWorkCorrectly_WhenInputIsRight_Data()
    {
        yield return new object[]
        {
            "123456",
            "999999",
            "999999",
        };
    }

    [Theory]
    [MemberData(nameof(PasswordChange_ShouldWorkCorrectly_WhenInputIsRight_Data))]
    public void PasswordChange_ShouldWorkCorrectly_WhenInputIsRight(string password, string newPassword,
        string newPasswordRepeat)
    {
        //Arrange
       
        var user = _entityGenerator.GenerateUser;
        user.SetPassword(_hashManager.CreateHash("123456"));
        //Act

        _userManager.ChangePassword(user, password, newPassword, newPasswordRepeat);

        //Assert
        Assert.Equal(user.Password, _hashManager.CreateHash(newPassword));
    }


    //*************************************************************//


    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync() => _resetDatabase();
}