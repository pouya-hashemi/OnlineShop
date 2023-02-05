using Microsoft.AspNetCore.Identity;
using OnlineShop.Domain.Common;
using OnlineShop.Domain.EntityPropertyConfigurations;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;


namespace OnlineShop.Domain.Entities;

public class User :IdentityUser<long>, IAuditableEntity
{
    public string UserTitle { get; private set; }
    public long CreatedUserId { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public long? ModifiedUserId { get; set; }
    public DateTime? ModifiedDateTime { get; set; }
    

    private User() { }

    public User(string username, string userTitle)
    {
        SetUsername(username);
        SetUserTitle(userTitle);
    }


    #region Validations

    private string ValidateUsername(string username)
    {
        if (String.IsNullOrWhiteSpace(username))
        {
            throw new NullOrEmptyException(nameof(username));
        }

        if (username.Length < UserPropertyConfiguration.UsernameMinLength)
        {
            throw new MinLengthException(nameof(username), UserPropertyConfiguration.UsernameMinLength);
        }

        if (username.Length > 50)
        {
            throw new MaxLengthException(nameof(username), UserPropertyConfiguration.UsernameMaxLength);
        }

        return username;
    }

    

    private string ValidateUserTitle(string userTitle)
    {
        if (String.IsNullOrWhiteSpace(userTitle))
        {
            throw new NullOrEmptyException(nameof(userTitle));
        }

        if (userTitle.Length > UserPropertyConfiguration.UserTitleMaxLength)
        {
            throw new MaxLengthException(nameof(userTitle), UserPropertyConfiguration.UserTitleMaxLength);
        }

        return userTitle;
    }

    #endregion

    #region Setters

    /// <summary>
    /// Validate, Trim  and change username property of user
    /// </summary>
    /// <param name="username"></param>
    internal void SetUsername(string username)
    {
        UserName = ValidateUsername(username);
    }


    /// <summary>
    /// validate and change userTitle property of user
    /// </summary>
    /// <param name="userTitle"></param>
    internal void SetUserTitle(string userTitle)
    {
        UserTitle = ValidateUserTitle(userTitle);
    }

    #endregion

   
}