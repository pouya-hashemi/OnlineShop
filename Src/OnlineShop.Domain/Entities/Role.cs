using OnlineShop.Domain.Common;
using OnlineShop.Domain.EntityPropertyConfigurations;
using OnlineShop.Domain.Exceptions;

namespace OnlineShop.Domain.Entities;

public class Role : EntityBase<int>
{
    public string Name { get; private set; }
    public ICollection<User> Users { get; set; }

    public Role(string name)
    {
        this.Name = ValidateName(name);
    }

    #region Validations

    private string ValidateName(string name)
    {
        if (String.IsNullOrWhiteSpace(name))
        {
            throw new NullOrEmptyException(nameof(name));
        }

        if (name.Length > RolePropertyConfiguration.NameMaxLength)
        {
            throw new MaxLengthException(nameof(name), RolePropertyConfiguration.NameMaxLength);
        }
        
        return name;
    }

    #endregion

    #region Setters

    internal void SetName(string name)
    {
        this.Name = ValidateName(name);
    }

    #endregion
}