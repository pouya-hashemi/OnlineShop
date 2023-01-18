using OnlineShop.Domain.Common;
using OnlineShop.Domain.EntityPropertyConfigurations;
using OnlineShop.Domain.Exceptions;

namespace OnlineShop.Domain.Entities;

public class Category : EntityBase<int>
{
    public string Name { get; private set; }
    public ICollection<Product> Products { get; private set; }


    public Category(string name)
    {
        SetName(name);
        Products = new List<Product>();
    }

    #region Validators

    private string ValidateName(string name)
    {
        if (String.IsNullOrWhiteSpace(name))
        {
            throw new NullOrEmptyException(nameof(name));
        }

        if (name.Length > CategoryPropertyConfiguration.NameMaxLength)
        {
            throw new MaxLengthException(nameof(name), CategoryPropertyConfiguration.NameMaxLength);
        }

        return name;
    }

    #endregion

    #region Setters

    /// <summary>
    /// validate, trim and change name property of this category
    /// </summary>
    /// <param name="name"></param>
    internal void SetName(string name)
    {
        Name = ValidateName(name).Trim();
    }

    #endregion
}