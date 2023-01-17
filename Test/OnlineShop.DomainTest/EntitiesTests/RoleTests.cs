using OnlineShop.Domain.EntityPropertyConfigurations;
using OnlineShop.Domain.Exceptions;
using OnlineShop.TestShareContent.DataGenerators;

namespace OnlineShop.DomainTest.EntitiesTests;

public class RoleTests
{
    private readonly EntityGenerator _entityGenerator;

    public RoleTests()
    {
        _entityGenerator = new EntityGenerator();
    }

    public static IEnumerable<object[]> SetName_ShouldThrowNullOrWhiteException_WhenNameIsNullOrWhiteSpace_Data()
    {
        yield return new object[]
        {
            ""
        };
        yield return new object[]
        {
            null
        };
        yield return new object[]
        {
            " "
        };
    }

    [Theory]
    [MemberData(nameof(SetName_ShouldThrowNullOrWhiteException_WhenNameIsNullOrWhiteSpace_Data))]
    public void SetName_ShouldThrowNullOrWhiteException_WhenNameIsNullOrWhiteSpace(string name)
    {
        //Arrange
        var role = _entityGenerator.GenerateRole;
        //Act
        var act = () => { role.SetName(name); };
        //Assert
        Assert.Throws<NullOrEmptyException>(act);
    }
    
    //***************************************************************//
    public static IEnumerable<object[]> SetName_ShouldThrowMaxLengthException_WhenNameMaxLengthIsWrong_Data()
    {
        yield return new object[]
        {
            new string(Enumerable.Repeat('a',RolePropertyConfiguration.NameMaxLength+1).ToArray())
        };
    }

    [Theory]
    [MemberData(nameof(SetName_ShouldThrowMaxLengthException_WhenNameMaxLengthIsWrong_Data))]
    public void SetName_ShouldThrowMaxLengthException_WhenNameMaxLengthIsWrong(string name)
    {
        //Arrange
        var role = _entityGenerator.GenerateRole;
        //Act
        var act = () => { role.SetName(name); };
        //Assert
        Assert.Throws<MaxLengthException>(act);
    }
    
    //***************************************************************//

}