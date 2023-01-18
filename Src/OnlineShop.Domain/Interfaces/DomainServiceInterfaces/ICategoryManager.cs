using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

public interface ICategoryManager
{
    Task<Category> CreateCategoryAsync(string name, CancellationToken cancellationToken = default);
    /// <summary>
    /// Change Category name to following name
    /// </summary>
    /// <param name="category"></param>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentNullException">category and name must have value</exception>
    /// <exception cref="AlreadyExistException">Duplicate Name</exception>
    Task ChangeNameAsync(Category category, string name, CancellationToken cancellationToken = default);
    /// <summary>
    /// Throws proper exception if data cant be deleted
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    Task IsDeletable(Category category);
}