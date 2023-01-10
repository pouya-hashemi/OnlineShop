using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

public interface IUserManager
{
    Task<User> CreateUserAsync(string username, string password, string passwordReEnter, string userTitle,IEnumerable<Role> roles,
        CancellationToken cancellationToken = default);

    Task ChangeUsernameAsync(User user, string username);
    void ChangeUserTitle(User user, string userTitle);
    void ChangePassword(User user, string password, string newPassword, string newPasswordReEnter);
    bool IsDeletable(User user);
    void AddRole(User user, Role role);
}