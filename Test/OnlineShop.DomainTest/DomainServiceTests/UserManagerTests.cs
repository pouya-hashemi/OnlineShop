using MockQueryable.Moq;
using Moq;
using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.DomainTest.Common;

namespace OnlineShop.DomainTest.DomainServiceTests;

public class UserManagerTests
{
    private readonly EntityGenerator _entityGenerator;
    public UserManagerTests()
    {
        _entityGenerator = new EntityGenerator();
    }

    public static IEnumerable<object[]> ShouldThrowAlreadyExistException_WhenUsernameExists_Data()
    {
        yield return new object[]
        {
            "PouyaHashemi"            
        };
    }
    [Theory]
    [MemberData(nameof(ShouldThrowAlreadyExistException_WhenUsernameExists_Data))]
    public void ChangeUsername_ShouldThrowAlreadyExistException_WhenUsernameExists(string username)
    {
        //Arrange
        var userInDb = _entityGenerator.GenerateUser;
        userInDb.SetUsername(username);
        userInDb.Id = 1;
        var dbContextMock = new Mock<IAppDbContext>();
        var userDbSetMock = new List<User>()
        {
            userInDb
        }.AsQueryable().BuildMockDbSet();
        
        dbContextMock
            .Setup(x => x.Users)
            .Returns(userDbSetMock.Object);

        var userManager = new UserManager(dbContextMock.Object);
        
        var userToUpdate = _entityGenerator.GenerateUser;
        userToUpdate.SetUsername(username);
        userToUpdate.Id = 2;
        //Act

        var act =async () => {await userManager.ChangeUsernameAsync(userToUpdate, username); };

        //Assert
        Assert.ThrowsAsync<AlreadyExistException>(act);
    }
    
    
    //*************************************************************//
}