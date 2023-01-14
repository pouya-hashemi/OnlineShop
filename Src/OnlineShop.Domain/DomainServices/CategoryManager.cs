using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Domain.DomainServices;

public class CategoryManager:ICategoryManager
{
    private readonly IAppDbContext _context;

    public CategoryManager(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Category> CreateCategoryAsync(string name, CancellationToken cancellationToken = default)
    {
        if (await CategoryNameExistAsync(name,cancellationToken: cancellationToken))
        {
            throw new AlreadyExistException(nameof(name), name);
        }

        var category = new Category(name);

        return category;
    }

/// <summary>
/// Change Category name to following name
/// </summary>
/// <param name="category"></param>
/// <param name="name"></param>
/// <param name="cancellationToken"></param>
/// <exception cref="ArgumentNullException">category and name must have value</exception>
/// <exception cref="AlreadyExistException">Duplicate Name</exception>
    public async Task ChangeNameAsync(Category category, string name,CancellationToken cancellationToken=default)
    {
        if (category is null)
        {
            throw new ArgumentNullException(nameof(category));
        }

        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (await CategoryNameExistAsync(name,category.Id,cancellationToken))
        {
            throw new AlreadyExistException(nameof(name), name);
        }
        
        category.SetName(name);
        
    }
/// <summary>
/// Throws proper exception if data cant be deleted
/// </summary>
/// <param name="category"></param>
/// <returns></returns>
public Task IsDeletable(Category category)
{
    return Task.CompletedTask;
}


private async Task<bool> CategoryNameExistAsync(string name,int? id=null,CancellationToken cancellationToken=default)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }
        
        var query = _context.Categories
            .Where(w=>w.Name==name.Trim())
            .AsNoTracking()
            .AsQueryable();

        if (id is not null)
        {
            query = query.Where(w => w.Id != id);
        }

        return await query.AnyAsync(cancellationToken);
    }
}