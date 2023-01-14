﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Interfaces;

public interface IAppDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<Category> Categories { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
     DatabaseFacade Database { get;  }
}