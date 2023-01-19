using OnlineShop.Domain.Common;
using OnlineShop.Domain.EntityPropertyConfigurations;
using OnlineShop.Domain.Exceptions;

namespace OnlineShop.Domain.Entities;

public class Cart : EntityBase<long>
{
    public Guid Token { get; private set; }
    public long ProductId { get; private set; }
    public Product Product { get; set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }


    public Cart(string token, Product product, int quantity, long price)
    {
        SetToken(token);
        SetProduct(product);
        SetQuantity(quantity);
        SetPrice(price);
    }

    #region Validations

    private Guid ValidateToken(string token)
    {
        if (!Guid.TryParse(token, out var validToken))
        {
            throw new NullOrEmptyException(nameof(token));
        }

        return validToken;
    }

    private Product ValidateProduct(Product product)
    {
        if (product is null)
        {
            throw new NullOrEmptyException(nameof(product));
        }

        return product;
    }

    private int ValidateQuantity(int quantity)
    {
        if (quantity < CartPropertyConfigurations.MinQuantity)
        {
            throw new LessThanException(nameof(quantity), CartPropertyConfigurations.MinQuantity);
        }

        return quantity;
    }

    private decimal ValidatePrice(decimal price)
    {
        if (price < CartPropertyConfigurations.MinPrice)
        {
            throw new LessThanException(nameof(price), CartPropertyConfigurations.MinPrice);
        }

        return price;
    }

    #endregion

    #region Setters

    /// <summary>
    /// validate and set Token property of cart
    /// </summary>
    /// <param name="token"></param>
    internal void SetToken(string token)
    {
        this.Token = ValidateToken(token);
    }

    /// <summary>
    /// Validate and sets product and product id of cart
    /// </summary>
    /// <param name="product"></param>
    internal void SetProduct(Product product)
    {
        this.Product = ValidateProduct(product);
        this.ProductId = product.Id;
    }

    /// <summary>
    /// Validate and sets quantity of cart
    /// </summary>
    /// <param name="quantity"></param>
    internal void SetQuantity(int quantity)
    {
        this.Quantity = ValidateQuantity(quantity);
    }

    /// <summary>
    /// Validate and sets price of cart
    /// </summary>
    /// <param name="price"></param>
    internal void SetPrice(decimal price)
    {
        this.Price = ValidatePrice(price);
    }

    #endregion
}