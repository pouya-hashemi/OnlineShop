using OnlineShop.Domain.Common;
using OnlineShop.Domain.EntityPropertyConfigurations;
using OnlineShop.Domain.Exceptions;

namespace OnlineShop.Domain.Entities;

public class Product : EntityBase<long>
{
    public string Name { get; private set; }
    public string ImageUrl { get; private set; }
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }
    public int CategoryId { get; private set; }
    public Category Category { get; private set; }
    
    private Product(){}
    public Product(string name, string imageUrl, decimal price, int quantity, Category category)
    {
        Name = ValidateName(name);
        ImageUrl = ValidateImageURl(imageUrl);
        Price = ValidatePrice(price);
        Quantity = ValidateQuantity(quantity);
        Category = ValidateCategory(category);
        CategoryId = category.Id;
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

    private string ValidateImageURl(string imageUrl)
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

    internal void SetName(string name)
    {
        Name = ValidateName(name);
    }

    internal void SetImageUrl(string imageUrl)
    {
        ImageUrl = ValidateImageURl(imageUrl);
    }

    internal void SetPrice(decimal price)
    {
        Price = ValidatePrice(price);
    }

    internal void SetQuantity(int quantity)
    {
        Quantity = ValidateQuantity(quantity);
    }

    internal void SetCategory(Category category)
    {
        Category = ValidateCategory(category);
        CategoryId = category.Id;
    }

    #endregion
}