
using OnlineShop.Domain.Common;
using OnlineShop.Domain.Exceptions;


namespace OnlineShop.Domain.Entities;
public class User:EntityBase<long>
{
    public string Username { get;private set; }
    public string Password { get;private set; }
    public string UserTitle { get;private set; }
    

    public User(string username,string password,string userTitle)
    {
        Username = ValidateUsername(username);
        Password = ValidatePassword(password);
        UserTitle = ValidateUserTitle(userTitle);
    }


    #region Validations
    
    private string ValidateUsername(string username)
    {
        if (String.IsNullOrWhiteSpace(username))
        {
            throw new NullOrEmptyException(nameof(username));
        }
        
        if (username.Length < 4)
        {
            throw new MinLengthException(nameof(username), 4);
        }
        if (username.Length>50)
        {
            throw new MaxLengthException(nameof(username), 50);
        }
        return username;
    }

    private string ValidatePassword(string password)
    {
        if (String.IsNullOrWhiteSpace(password))
        {
            throw new NullOrEmptyException(nameof(password));
        }

        if (password.Length!=256)
        {
            throw new LengthException(nameof(password), 256);
        }

        return password;
    }

    private string ValidateUserTitle(string userTitle)
    {
        if (String.IsNullOrWhiteSpace(userTitle))
        {
            throw new NullOrEmptyException(nameof(userTitle));
        }

        if (userTitle.Length>50)
        {
            throw new MaxLengthException(nameof(userTitle), 50);
        }

        return userTitle;
    }
    
    #endregion
    
    #region Setters
    
    internal void SetUsername(string username)
    {
        Username = ValidateUsername(username);
    }

    internal void SetPassword(string password)
    {
        Password = ValidatePassword(password);
    }
    internal void SetUserTitle(string userTitle)
    {
        UserTitle = ValidateUserTitle(userTitle);
    }

    #endregion
}