using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.EntityPropertyConfigurations;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Domain.DomainServices;

public class UserManager : IUserManager
{
    private readonly IAppDbContext _dbContext;
    private readonly HashManager _hashManager;

    public UserManager(IAppDbContext dbContext, HashManager hashManager)
    {
        _dbContext = dbContext;
        _hashManager = hashManager;
    }

    /// <summary>
    /// validates and create a valid user entity
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="passwordReEnter"></param>
    /// <param name="userTitle"></param>
    /// <param name="roles"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>a valid user entity</returns>
    /// <exception cref="AlreadyExistException">userName must be uniq </exception>
    public async Task<User> CreateUserAsync(string username, string password, string passwordReEnter, string userTitle
        , IEnumerable<Role> roles
        , CancellationToken cancellationToken = default)
    {
        if (await UsernameExistAsync(username, cancellationToken: cancellationToken))
        {
            throw new AlreadyExistException(nameof(username), username);
        }

        ValidatePasswordReEnter(password, passwordReEnter);

        ValidatePassword(password);

        var user = new User(username, HashPassword(password), userTitle);

        AddRole(user, roles);

        return user;
    }


    #region Validate

    private async Task<bool> UsernameExistAsync(string username, long? userId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Users
            .Where(w => w.Username == username)
            .AsQueryable();
        if (userId != null)
        {
            query = query
                .Where(w => w.Id != userId);
        }

        return await query.AsNoTracking().AnyAsync(cancellationToken);
    }

    private void ValidatePasswordReEnter(string password, string passwordReEnter)
    {
        if (password != passwordReEnter)
        {
            throw new BadRequestException("New password doesn't match the password repeat.");
        }
    }

    private void CheckPassword(User user, string password)
    {
        if (!String.IsNullOrWhiteSpace(user.Password) && user.Password != _hashManager.CreateHash(password))
        {
            throw new BadRequestException("your password is wrong.");
        }
    }

    private string ValidatePassword(string password)
    {
        if (String.IsNullOrWhiteSpace(password))
        {
            throw new NullOrEmptyException(nameof(password));
        }

        if (password.Length < UserPropertyConfiguration.PasswordMinLength)
        {
            throw new MinLengthException(nameof(password), UserPropertyConfiguration.PasswordMinLength);
        }

        if (password.Length > UserPropertyConfiguration.PasswordMaxLength)
        {
            throw new MaxLengthException(nameof(password), UserPropertyConfiguration.PasswordMaxLength);
        }

        return password;
    }

    #endregion

    #region Setter

    /// <summary>
    /// validate and change username of user
    /// </summary>
    /// <param name="user"></param>
    /// <param name="username"></param>
    /// <exception cref="ArgumentNullException">user must have value</exception>
    /// <exception cref="AlreadyExistException">username must be unique</exception>
    public async Task ChangeUsernameAsync(User user, string username)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (await UsernameExistAsync(username, user.Id))
        {
            throw new AlreadyExistException(nameof(username), username);
        }

        user.SetUsername(username);
    }

    /// <summary>
    /// validate and changes userTitle of user
    /// </summary>
    /// <param name="user"></param>
    /// <param name="userTitle"></param>
    /// <exception cref="ArgumentNullException">user must have value</exception>
    public void ChangeUserTitle(User user, string userTitle)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        user.SetUserTitle(userTitle);
    }

    /// <summary>
    /// validate , hash and change password of user
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <param name="newPassword"></param>
    /// <param name="newPasswordReEnter"></param>
    /// <exception cref="ArgumentNullException">user must have value</exception>
    public void ChangePassword(User user, string password, string newPassword, string newPasswordReEnter)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        ValidatePasswordReEnter(newPassword, newPasswordReEnter);

        ValidatePassword(newPassword);

        CheckPassword(user, password);

        user.SetPassword(HashPassword(newPassword));
    }

    private string HashPassword(string password)
    {
        return _hashManager.CreateHash(password);
    }

    #endregion

    /// <summary>
    /// determines if user is deletable or not
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException">User does not exists in database</exception>
    public bool IsDeletable(User user)
    {
        if (user is null)
        {
            throw new NotFoundException(nameof(User));
        }

        return true;
    }

    /// <summary>
    /// Adds new role to user
    /// </summary>
    /// <param name="user"></param>
    /// <param name="role"></param>
    /// <exception cref="ArgumentNullException">user must have value</exception>
    public void AddRole(User user, Role role)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (role is null)
        {
            throw new ArgumentNullException(nameof(role));
        }

        user.Roles.Add(role);
    }

    /// <summary>
    /// Adds multiple role to user
    /// </summary>
    /// <param name="user"></param>
    /// <param name="roles"></param>
    /// <exception cref="ArgumentNullException">user must have value</exception>
    public void AddRole(User user, IEnumerable<Role> roles)
    {
        if (roles is null)
        {
            throw new ArgumentNullException(nameof(roles));
        }

        foreach (var role in roles)
        {
            AddRole(user, role);
        }
    }
}