using OnlineShop.Domain.Common;
using OnlineShop.Domain.EntityPropertyConfigurations;
using OnlineShop.Domain.Exceptions;


namespace OnlineShop.Domain.Entities;

public class User : EntityBase<long>
{
    public string Username { get; private set; }
    public string Password { get; private set; }
    public string UserTitle { get; private set; }
    public ICollection<Role> Roles { get; set; }


    public User(string username, string password, string userTitle)
    {
        SetUsername(username);
        SetPassword(password);
        SetUserTitle(userTitle);
        Roles = new List<Role>();
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

    private string ValidatePassword(string password)
    {
        if (String.IsNullOrWhiteSpace(password))
        {
            throw new NullOrEmptyException(nameof(password));
        }

        if (password.Length != UserPropertyConfiguration.PasswordHashLength)
        {
            throw new LengthException(nameof(password), UserPropertyConfiguration.PasswordHashLength);
        }

        return password;
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
        Username = ValidateUsername(username);
    }

    /// <summary>
    /// Validate and change hashed password property of user
    /// </summary>
    /// <param name="password"></param>
    internal void SetPassword(string password)
    {
        Password = ValidatePassword(password);
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