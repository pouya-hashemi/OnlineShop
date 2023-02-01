using Microsoft.AspNetCore.Identity;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Exceptions.BaseExceptions;

namespace OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

public interface IUserManager
{
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
    Task<User> CreateUserAsync(string username, string password, string passwordReEnter, string userTitle
        , CancellationToken cancellationToken = default);
    /// <summary>
    /// validate and change username of user
    /// </summary>
    /// <param name="user"></param>
    /// <param name="username"></param>
    /// <exception cref="ArgumentNullException">user must have value</exception>
    /// <exception cref="AlreadyExistException">username must be unique</exception>
    Task ChangeUsernameAsync(User user, string username);
    /// <summary>
    /// validate and changes userTitle of user
    /// </summary>
    /// <param name="user"></param>
    /// <param name="userTitle"></param>
    /// <exception cref="ArgumentNullException">user must have value</exception>
    void ChangeUserTitle(User user, string userTitle);

    /// <summary>
    /// validate , hash and change password of user
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <param name="newPassword"></param>
    /// <param name="newPasswordReEnter"></param>
    /// <exception cref="ArgumentNullException">user must have value</exception>
    Task ChangePassword(User user, string password, string newPassword, string newPasswordReEnter,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// determines if user is deletable or not
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException">User does not exists in database</exception>
    bool IsDeletable(User user);

    /// <summary>
    /// Adds new role to user
    /// </summary>
    /// <param name="user"></param>
    /// <param name="role"></param>
    /// <exception cref="ArgumentNullException">user must have value</exception>
    IdentityUserRole<long> CreateUserRole(User user, Role role);

}