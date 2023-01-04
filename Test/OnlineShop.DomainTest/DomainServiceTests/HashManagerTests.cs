using OnlineShop.Domain.DomainServices;
using OnlineShop.Domain.EntityPropertyConfigurations;

namespace OnlineShop.DomainTest.DomainServiceTests;

public class HashManagerTests
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void CreateHash_ShouldThrowArgumentException_WhenTextIsNullOrWhiteSpace(string text)
    {
        //Arrange
        var hashManager = new HashManager();
        //Act
        var act = () => { hashManager.CreateHash(text); };
        //Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void CreateHash_ShouldReturnCorrectLengthString()
    {
        //Arrange
        var text = "123456";
        var hashManager = new HashManager();
        //Act
        var hashedText = hashManager.CreateHash(text);
        //Assert
        Assert.Equal(UserPropertyConfiguration.PasswordHashLength, hashedText.Length);
    }
}