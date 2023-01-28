using OnlineShop.Domain.Common;
using OnlineShop.Domain.EntityPropertyConfigurations;
using OnlineShop.Domain.Exceptions;

namespace OnlineShop.Domain.Entities;

public class Product : EntityBase<long>
{
    public string Name { get; private set; }
    public string ImagePath { get; private set; }
    public string ImageUrl { get;private set; }
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }
    public int CategoryId { get; private set; }
    public Category Category { get; private set; }
    public ICollection<Cart> Carts { get; set; }

    private Product()
    {
    }

    public Product(string name, string imagePath, decimal price, int quantity, Category category,string imageUrl)
    {
        SetName(name);
        SetImagePath(imagePath);
        SetPrice(price);
        SetQuantity(quantity);
        SetCategory(category);
        SetImageUrl(imageUrl);
        Carts = new List<Cart>();
    }

    

    #region Validators

    private int ValidateQuantity(int quantity)
    {
        if (quantity < ProductPropertyConfiguration.QuantityMinValue)
        {
            throw new LessThanException(nameof(quantity), ProductPropertyConfiguration.QuantityMinValue);
        }

        return quantity;
    }

    private Category ValidateCategory(Category category)
    {
        if (category is null)
        {
            throw new NullOrEmptyException(nameof(category));
        }

        return category;
    }

    private decimal ValidatePrice(decimal price)
    {
        if (price < ProductPropertyConfiguration.PriceMinValue)
        {
            throw new LessThanException(nameof(price), ProductPropertyConfiguration.PriceMinValue);
        }

        return price;
    }

    private string ValidateName(string name)
    {
        if (String.IsNullOrWhiteSpace(name))
        {
            throw new NullOrEmptyException(nameof(name));
        }

        if (name.Length > ProductPropertyConfiguration.NameMaxLength)
        {
            throw new MaxLengthException(nameof(name), ProductPropertyConfiguration.NameMaxLength);
        }

        return name;
    }

    private string ValidateImagePath(string imagePath)
    {
        if (String.IsNullOrWhiteSpace(imagePath))
        {
            throw new NullOrEmptyException("image");
        }

        if (imagePath.Length > ProductPropertyConfiguration.ImagePathMaxLength)
        {
            throw new MaxLengthException(nameof(imagePath), ProductPropertyConfiguration.ImagePathMaxLength);
        }

        return imagePath;
    }
    
    private string ValidateImageUrl(string imageUrl)
    {
        if (String.IsNullOrWhiteSpace(imageUrl))
        {
            throw new NullOrEmptyException("image");
        }

        if (imageUrl.Length > ProductPropertyConfiguration.ImageUrlMaxLength)
        {
            throw new MaxLengthException(nameof(imageUrl), ProductPropertyConfiguration.ImageUrlMaxLength);
        }

        return imageUrl;
    }

    #endregion

    #region Setters

    /// <summary>
    /// Validate, Trim and Change name property of product
    /// </summary>
    /// <param name="name"></param>
    internal void SetName(string name)
    {
        Name = ValidateName(name).Trim();
    }

    /// <summary>
    /// Validate, change ImagePath property of product
    /// </summary>
    /// <param name="imageUrl"></param>
    internal void SetImagePath(string imagePath)
    {
        ImagePath = ValidateImagePath(imagePath);
    }

    /// <summary>
    /// Validate and change price property of product
    /// </summary>
    /// <param name="price"></param>
    internal void SetPrice(decimal price)
    {
        Price = ValidatePrice(price);
    }

    /// <summary>
    /// Validate and change quantity property of product
    /// </summary>
    /// <param name="quantity"></param>
    internal void SetQuantity(int quantity)
    {
        Quantity = ValidateQuantity(quantity);
    }

    /// <summary>
    /// validate and change category and categoryId property of product
    /// </summary>
    /// <param name="category"></param>
    internal void SetCategory(Category category)
    {
        Category = ValidateCategory(category);
        CategoryId = category.Id;
    }
    /// <summary>
    /// validate and change imageUrl property of product
    /// </summary>
    /// <param name="imageUrl"></param>
    internal void SetImageUrl(string imageUrl)
    {
        ImageUrl = ValidateImageUrl(imageUrl);
    }

    

    #endregion
}