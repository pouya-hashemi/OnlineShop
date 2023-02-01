using Microsoft.AspNetCore.Identity;
using OnlineShop.Domain.Common;
using OnlineShop.Domain.EntityPropertyConfigurations;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Domain.Entities;

public class Role : IdentityRole<long>,IAuditableEntity
{
    public Role(string name)
    {
        SetName(name);
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

    /// <summary>
    /// Validate and change name of role
    /// </summary>
    /// <param name="name"></param>
    internal void SetName(string name)
    {
        this.Name = ValidateName(name);
    }

    #endregion

    public long CreatedUserId { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public long? ModifiedUserId { get; set; }
    public DateTime? ModifiedDateTime { get; set; }
}