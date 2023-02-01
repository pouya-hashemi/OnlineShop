using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Interfaces;

public interface IAppDbContext
{
    DbSet<Category> Categories { get; set; }
    DbSet<Product> Products { get; set; }
    DbSet<Cart> Carts { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<IdentityUserRole<long>> UserRoles { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
     DatabaseFacade Database { get;  }
}