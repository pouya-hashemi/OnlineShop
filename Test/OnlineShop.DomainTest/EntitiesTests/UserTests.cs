
using OnlineShop.Domain.EntityPropertyConfigurations;
using OnlineShop.Domain.Exceptions;
using OnlineShop.TestShareContent.DataGenerators;

namespace OnlineShop.DomainTest.EntitiesTests;

public class UserTests
{
    private readonly EntityGenerator _entityGenerator;

    public UserTests()
    {
        _entityGenerator = new EntityGenerator();
    }
    //******************************************************************//
    public static IEnumerable<object[]> ShouldThrowLengthException_WhenUsernameLengthIsWrongData()
    {
        yield return new object[]
        {
            new string(Enumerable.Repeat('*', UserPropertyConfiguration.UsernameMinLength-1).ToArray()),
            typeof(MinLengthException)
        };
        yield return new object[]
        {
            new string(Enumerable.Repeat('*', UserPropertyConfiguration.UsernameMaxLength+1).ToArray()),
            typeof(MaxLengthException)
        };
        yield return new object[]
        {
            null,
            typeof(NullOrEmptyException)
        };
        yield return new object[]
        {
            string.Empty,
            typeof(NullOrEmptyException)
        };
        yield return new object[]
        {
            " ",
            typeof(NullOrEmptyException)
        };
    }

    [Theory]
    [MemberData(nameof(ShouldThrowLengthException_WhenUsernameLengthIsWrongData))]
    public void ShouldThrowLengthException_WhenUsernameLengthIsWrong(string username, Type exceptionType)
    {
        //Arrange
        var user = _entityGenerator.GenerateUser;
        //Act
        var act = () => { user.SetUsername(username); };
        //Assert
        Assert.Throws(exceptionType, act);
    }
    //******************************************************************//
    public static IEnumerable<object[]> ShouldThrowLengthException_WhenUserTitleLengthIsWrongData()
    {
        yield return new object[]
        {
            new string(Enumerable.Repeat('*', UserPropertyConfiguration.UserTitleMaxLength+1).ToArray()),
            typeof(MaxLengthException)
        };
        yield return new object[]
        {
            null,
            typeof(NullOrEmptyException)
        };
        yield return new object[]
        {
            string.Empty,
            typeof(NullOrEmptyException)
        };
        yield return new object[]
        {
            " ",
            typeof(NullOrEmptyException)
        };
    }

    [Theory]
    [MemberData(nameof(ShouldThrowLengthException_WhenUserTitleLengthIsWrongData))]
    public void ShouldThrowLengthException_WhenUserTitleLengthIsWrong(string userTitle, Type exceptionType)
    {
        //Arrange
        var user = _entityGenerator.GenerateUser;
        //Act
        var act = () => { user.SetUserTitle(userTitle); };
        //Assert
        Assert.Throws(exceptionType, act);
    }
    //******************************************************************//
    public static IEnumerable<object[]> ShouldChangeUsername_WhenUsernameIsCorrectData()
    {
        yield return new object[]
        {
            new string(Enumerable.Repeat('*', UserPropertyConfiguration.UsernameMaxLength).ToArray()),
        };
        yield return new object[]
        {
            new string(Enumerable.Repeat('*', UserPropertyConfiguration.UsernameMinLength).ToArray()),
        };
        yield return new object[]
        {
            "Pouya.Hashemi"
        };
        
    }

    [Theory]
    [MemberData(nameof(ShouldChangeUsername_WhenUsernameIsCorrectData))]
    public void ShouldChangeUsername_WhenUsernameIsCorrect(string username)
    {
        //Arrange
        var user = _entityGenerator.GenerateUser;
        //Act
         user.SetUsername(username);
        //Assert
        Assert.Equal(username,user.UserName);
    }
    //******************************************************************//
   
    public static IEnumerable<object[]> ShouldChangeUserTitle_WhenUserTitleIsCorrect_Data()
    {
        yield return new object[]
        {
            new string(Enumerable.Repeat('*', UserPropertyConfiguration.UserTitleMaxLength).ToArray()),
        };
        yield return new object[]
        {
            new string(Enumerable.Repeat('*', UserPropertyConfiguration.UserTitleMinLength).ToArray()),
        };
        yield return new object[]
        {
            "Pouya Hashemi"
        };

    }

    [Theory]
    [MemberData(nameof(ShouldChangeUserTitle_WhenUserTitleIsCorrect_Data))]
    public void ShouldChangeUserTitle_WhenUserTitleIsCorrect(string userTitle)
    {
        //Arrange
        var user = _entityGenerator.GenerateUser;
        //Act
         user.SetUserTitle(userTitle);
        //Assert
        Assert.Equal(userTitle,user.UserTitle);
    }
    //******************************************************************//
    
}