using OnlineShop.Domain.EntityPropertyConfigurations;
using OnlineShop.Domain.Exceptions;
using OnlineShop.TestShareContent.DataGenerators;


namespace OnlineShop.DomainTest.EntitiesTests;

public class CategoryTests
{
    private readonly EntityGenerator _entityGenerator;
    public CategoryTests()
    {
        _entityGenerator = new EntityGenerator();
    }
    public static IEnumerable<object[]> SetName_ShouldThrowNullOrEmptyException_WhenInputIsNullOrEmptyCorrect_Data()
    {
        yield return new object[]
        {
            null
        };
        yield return new object[]
        {
            string.Empty
        };
        yield return new object[]
        {
            " "
        };
    }
    [Theory]
    [MemberData(nameof(SetName_ShouldThrowNullOrEmptyException_WhenInputIsNullOrEmptyCorrect_Data))]
    public void SetName_ShouldThrowNullOrEmptyException_WhenInputIsNullOrEmptyCorrect(string name)
    {
        //Arrange
        var category = _entityGenerator.GenerateCategory;
        //Act
        var act = () => {category.SetName(name); };
        //Assert
        Assert.Throws<NullOrEmptyException>(act);
    }
    //***********************************************************//
    public static IEnumerable<object[]> SetName_ShouldThrowMaxLengthException_WhenInputIsLong_Data()
    {
        yield return new object[]
        {
            new string(Enumerable.Repeat('a',CategoryPropertyConfiguration.NameMaxLength+1).ToArray())
        };
    }
    [Theory]
    [MemberData(nameof(SetName_ShouldThrowMaxLengthException_WhenInputIsLong_Data))]
    public void SetName_ShouldThrowMaxLengthException_WhenInputIsLong(string name)
    {
        //Arrange
        var category = _entityGenerator.GenerateCategory;
        //Act
        var act = () => {category.SetName(name); };
        //Assert
        Assert.Throws<MaxLengthException>(act);
    }
}