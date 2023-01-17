using OnlineShop.Domain.Entities;
using OnlineShop.Domain.EntityPropertyConfigurations;
using OnlineShop.Domain.Exceptions;
using OnlineShop.DomainTest.Common;

namespace OnlineShop.DomainTest.EntitiesTests;

public class ProductTests
{
    private readonly EntityGenerator _entityGenerator;

    public ProductTests()
    {
        _entityGenerator = new EntityGenerator();
    }

    public static IEnumerable<object[]> SetName_ShouldThrowNullOrEmptyException_WhenNameIsNullOrWhiteSpace_Data()
    {
        yield return new object[]
        {
            null
        };
        yield return new object[]
        {
            ""
        };
        yield return new object[]
        {
            " "
        };
    }

    [Theory]
    [MemberData(nameof(SetName_ShouldThrowNullOrEmptyException_WhenNameIsNullOrWhiteSpace_Data))]
    public void SetName_ShouldThrowNullOrEmptyException_WhenNameIsNullOrWhiteSpace(string name)
    {
        //Arrange
        var product = _entityGenerator.GenerateProduct;
        //Act
        var act = () => { product.SetName(name); };
        //Assert
        Assert.Throws<NullOrEmptyException>(act);
    }

    //****************************************************************//
    public static IEnumerable<object[]> SetName_ShouldThrowMaxLengthException_WhenNameIsTooLong_Data()
    {
        yield return new object[]
        {
            new string(Enumerable.Repeat('a', ProductPropertyConfiguration.NameMaxLength + 1).ToArray())
        };
    }

    [Theory]
    [MemberData(nameof(SetName_ShouldThrowMaxLengthException_WhenNameIsTooLong_Data))]
    public void SetName_ShouldThrowMaxLengthException_WhenNameIsTooLong(string name)
    {
        //Arrange
        var product = _entityGenerator.GenerateProduct;
        //Act
        var act = () => { product.SetName(name); };
        //Assert
        Assert.Throws<MaxLengthException>(act);
    }

    //****************************************************************//
    public static IEnumerable<object[]> SetName_ShouldChangeName_WhenDataIsCorrect_Data()
    {
        yield return new object[]
        {
            new string(Enumerable.Repeat('a', ProductPropertyConfiguration.NameMaxLength).ToArray())
        };
        yield return new object[]
        {
            new string(Enumerable.Repeat('a', ProductPropertyConfiguration.NameMinLength).ToArray())
        };
    }

    [Theory]
    [MemberData(nameof(SetName_ShouldChangeName_WhenDataIsCorrect_Data))]
    public void SetName_ShouldChangeName_WhenDataIsCorrect(string name)
    {
        //Arrange
        var product = _entityGenerator.GenerateProduct;
        //Act
        product.SetName(name);
        //Assert
        Assert.Equal(name, product.Name);
    }

    //****************************************************************//
    public static IEnumerable<object[]>
        SetImageUrl_ShouldThrowNullOrEmptyException_WhenImageUrlIsNullOrWhiteSpace_Data()
    {
        yield return new object[]
        {
            null
        };
        yield return new object[]
        {
            ""
        };
        yield return new object[]
        {
            " "
        };
    }

    [Theory]
    [MemberData(nameof(SetImageUrl_ShouldThrowNullOrEmptyException_WhenImageUrlIsNullOrWhiteSpace_Data))]
    public void SetImageUrl_ShouldThrowNullOrEmptyException_WhenImageUrlIsNullOrWhiteSpace(string imageUrl)
    {
        //Arrange
        var product = _entityGenerator.GenerateProduct;
        //Act
        var act = () => { product.SetImageUrl(imageUrl); };
        //Assert
        Assert.Throws<NullOrEmptyException>(act);
    }

    //****************************************************************//
    public static IEnumerable<object[]> SetImageUrl_ShouldThrowMaxLengthException_WhenImageUrlIsTooLong_Data()
    {
        yield return new object[]
        {
            new string(Enumerable.Repeat('a', ProductPropertyConfiguration.ImageUrlMaxLength + 1).ToArray())
        };
    }

    [Theory]
    [MemberData(nameof(SetImageUrl_ShouldThrowMaxLengthException_WhenImageUrlIsTooLong_Data))]
    public void SetImageUrl_ShouldThrowMaxLengthException_WhenImageUrlIsTooLong(string imageUrl)
    {
        //Arrange
        var product = _entityGenerator.GenerateProduct;
        //Act
        var act = () => { product.SetImageUrl(imageUrl); };
        //Assert
        Assert.Throws<MaxLengthException>(act);
    }

    //****************************************************************//
    public static IEnumerable<object[]> SetImageUrl_ShouldChangeImageUrl_WhenDataIsCorrect_Data()
    {
        yield return new object[]
        {
            new string(Enumerable.Repeat('a', ProductPropertyConfiguration.ImageUrlMaxLength).ToArray())
        };
        yield return new object[]
        {
            new string(Enumerable.Repeat('a', ProductPropertyConfiguration.ImageUrlMinLength).ToArray())
        };
    }

    [Theory]
    [MemberData(nameof(SetImageUrl_ShouldChangeImageUrl_WhenDataIsCorrect_Data))]
    public void SetImageUrl_ShouldChangeImageUrl_WhenDataIsCorrect(string imageUrl)
    {
        //Arrange
        var product = _entityGenerator.GenerateProduct;
        //Act
        product.SetImageUrl(imageUrl);
        //Assert
        Assert.Equal(imageUrl, product.ImageUrl);
    }

    //****************************************************************//
    public static IEnumerable<object[]> SetPrice_ShouldThrowLessThanException_WhenMinPriceIsNotCorrect_Data()
    {
        yield return new object[]
        {
            ProductPropertyConfiguration.PriceMinValue - 1
        };
    }

    [Theory]
    [MemberData(nameof(SetPrice_ShouldThrowLessThanException_WhenMinPriceIsNotCorrect_Data))]
    public void SetPrice_ShouldThrowLessThanException_WhenMinPriceIsNotCorrect(decimal price)
    {
        //Arrange
        var product = _entityGenerator.GenerateProduct;
        //Act
        var act = () => { product.SetPrice(price); };
        //Assert
        Assert.Throws<LessThanException>(act);
    }
    //**********************************************//
    public static IEnumerable<object[]> SetPrice_ShouldChangePrice_WhenDataIsCorrect_Data()
    {
        yield return new object[]
        {
            ProductPropertyConfiguration.PriceMinValue
        };
    }

    [Theory]
    [MemberData(nameof(SetPrice_ShouldChangePrice_WhenDataIsCorrect_Data))]
    public void SetPrice_ShouldChangePrice_WhenDataIsCorrect(decimal price)
    {
        //Arrange
        var product = _entityGenerator.GenerateProduct;
        //Act
        product.SetPrice(price);
        //Assert
        Assert.Equal(price, product.Price);
    }

    //****************************************************************//

    public static IEnumerable<object[]> SetQuantity_ShouldThrowLessThanException_WhenMinQuantityIsNotCorrect_Data()
    {
        yield return new object[]
        {
            ProductPropertyConfiguration.QuantityMinValue-1
        };
    }
    [Theory]
    [MemberData(nameof(SetQuantity_ShouldThrowLessThanException_WhenMinQuantityIsNotCorrect_Data))]
    public void SetQuantity_ShouldThrowLessThanException_WhenMinQuantityIsNotCorrect(int quantity)
    {
        //Arrange
        var product = _entityGenerator.GenerateProduct;
        //Act
        var act = () => { product.SetQuantity(quantity); };
        //Assert
        Assert.Throws<LessThanException>(act);
    }
    
    //**********************************************//
    public static IEnumerable<object[]> SetQuantity_ShouldChangeQuantity_WhenDataIsCorrect_Data()
    {
        yield return new object[]
        {
            ProductPropertyConfiguration.QuantityMinValue
        };
    }

    [Theory]
    [MemberData(nameof(SetQuantity_ShouldChangeQuantity_WhenDataIsCorrect_Data))]
    public void SetQuantity_ShouldChangeQuantity_WhenDataIsCorrect(int quantity)
    {
        //Arrange
        var product = _entityGenerator.GenerateProduct;
        //Act
        product.SetQuantity(quantity);
        //Assert
        Assert.Equal(quantity, product.Quantity);
    }

    //****************************************************************//
    public static IEnumerable<object[]> SetCategory_ShouldThrowNullOEmptyException_WhenCategoryIsNull_Data()
    {
        yield return new object[]
        {
            null
        };
    }
    [Theory]
    [MemberData(nameof(SetCategory_ShouldThrowNullOEmptyException_WhenCategoryIsNull_Data))]
    public void SetCategory_ShouldThrowNullOEmptyException_WhenCategoryIsNull(Category category)
    {
        //Arrange
        var product = _entityGenerator.GenerateProduct;
        //Act
        var act = () => { product.SetCategory(category); };
        //Assert
        Assert.Throws<NullOrEmptyException>(act);
    }
    //****************************************************************//
    public static IEnumerable<object[]> SetCategory_ShouldChangeCategory_WhenCategoryIsNotNull_Data()
    {
        yield return new object[]
        {
            new EntityGenerator().GenerateCategory
        };
    }
    [Theory]
    [MemberData(nameof(SetCategory_ShouldChangeCategory_WhenCategoryIsNotNull_Data))]
    
    public void SetCategory_ShouldChangeCategory_WhenCategoryIsNotNull(Category category)
    {
        //Arrange
        var product = _entityGenerator.GenerateProduct;
        //Act
        product.SetCategory(category);
        //Assert
        Assert.Equivalent(category, product.Category);
    }
    
}