using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Domain.DomainServices;

public class RoleManager : IRoleManager
{
    private readonly IAppDbContext _context;
    private readonly RoleManager<Role> _roleManager;

    public RoleManager(IAppDbContext context,
        RoleManager<Role> roleManager)
    {
        _context = context;
        _roleManager = roleManager;
    }

    /// <summary>
    /// creates a valid role
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>a valid role</returns>
    /// <exception cref="AlreadyExistException">name must be uniq</exception>
    public async Task<Role> CreateRoleAsync(string name, CancellationToken cancellationToken = default)
    {
        if (await NameExistsAsync(name, cancellationToken: cancellationToken))
        {
            throw new AlreadyExistException("Role name", name);
        }

        var role = new Role(name);


        return role;
    }

    /// <summary>
    /// validates and change name of role
    /// </summary>
    /// <param name="role"></param>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentNullException">role must have value</exception>
    /// <exception cref="AlreadyExistException">name must be uniq</exception>
    public async Task ChangeNameAsync(Role role, string name, CancellationToken cancellationToken = default)
    {
        if (role is null)
        {
            throw new ArgumentNullException(nameof(role));
        }

        if (await NameExistsAsync(name, cancellationToken: cancellationToken))
        {
            throw new AlreadyExistException("Role name", name);
        }

        role.SetName(name);
    }

    /// <summary>
    /// check if role can be deleted from database
    /// </summary>
    /// <param name="role"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>returns true if data can be deleted</returns>
    /// <exception cref="NotFoundException">Role must be available in Database</exception>
    public async Task<bool> IsDeletableAsync(Role role, CancellationToken cancellationToken = default)
    {
        if (role is null)
        {
            throw new NotFoundException(nameof(role));
        }

        return true;
    }

    #region Validation

    private async Task<bool> NameExistsAsync(string roleName, int? id = null,
        CancellationToken cancellationToken = default)
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
}