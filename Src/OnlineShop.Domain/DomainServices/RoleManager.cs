using System.Data;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Domain.DomainServices;

public class RoleManager : IRoleManager,IDisposable
{
    private readonly IAppDbContext _context;

    public RoleManager(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Role> CreateRoleAsync(string name,CancellationToken cancellationToken=default)
    {
        if (await NameExistsAsync(name,cancellationToken:cancellationToken))
        {
            throw new AlreadyExistException("Role name", name);
        }

        var role = new Role(name);
        

        return role;
    }

    public async Task ChangeNameAsync(Role role, string name, CancellationToken cancellationToken = default)
    {
        if (role is null)
        {
            throw new ArgumentNullException(nameof(role));
        }

        if (await NameExistsAsync(name,cancellationToken: cancellationToken))
        {
            throw new AlreadyExistException("Role name", name);
        }

        role.SetName(name);
    }


    public async Task<bool> IsDeletableAsync(Role role,CancellationToken cancellationToken=default)
    {
        if (role is null)
        {
            throw new NotFoundException(nameof(role));
        }

        return true;
    }
    
    #region Validation

    private async Task<bool> NameExistsAsync(string roleName, int? id = null,CancellationToken cancellationToken=default)
    {
        var query = _context.Roles
            .Where(w => w.Name.ToLower() == roleName.ToLower())
            .AsQueryable();
        if (id != null)
        {
            query = query.Where(w => w.Id != id);
        }

        return await query.AnyAsync(cancellationToken);
    }

    #endregion

    public void Dispose()
    {
        //Free resources
    }
}