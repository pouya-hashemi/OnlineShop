using OnlineShop.Domain.Common;
using OnlineShop.Domain.EntityPropertyConfigurations;
using OnlineShop.Domain.Exceptions;

namespace OnlineShop.Domain.Entities;

public class Product : EntityBase<long>
{
    public string Name { get; private set; }
    public string ImageUrl { get;private set; }

    public Product(string name,string imageUrl)
    {
        Name = ValidateName(name);
        ImageUrl = ValidateImageURl(imageUrl);
        ;
    }

    #region Validators

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

    #endregion
}