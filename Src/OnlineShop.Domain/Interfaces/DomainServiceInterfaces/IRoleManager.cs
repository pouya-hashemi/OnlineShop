using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

public interface IRoleManager
{
     Task<Role> CreateRoleAsync(string name,CancellationToken cancellationToken=default);
     Task ChangeNameAsync(Role role, string name,CancellationToken cancellationToken=default);
     Task<bool> IsDeletableAsync(Role role, CancellationToken cancellationToken = default);
}