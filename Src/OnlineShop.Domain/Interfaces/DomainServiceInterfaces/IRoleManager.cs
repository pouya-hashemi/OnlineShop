using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

public interface IRoleManager
{
    /// <summary>
    /// creates a valid role
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>a valid role</returns>
    /// <exception cref="AlreadyExistException">name must be uniq</exception>
    Task<Role> CreateRoleAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// validates and change name of role
    /// </summary>
    /// <param name="role"></param>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentNullException">role must have value</exception>
    /// <exception cref="AlreadyExistException">name must be uniq</exception>
    Task ChangeNameAsync(Role role, string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// check if role can be deleted from database
    /// </summary>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>returns true if data can be deleted</returns>
    /// <exception cref="NotFoundException">Role must be available in Database</exception>
    Task<bool> IsDeletableAsync(Role role, CancellationToken cancellationToken = default);
}