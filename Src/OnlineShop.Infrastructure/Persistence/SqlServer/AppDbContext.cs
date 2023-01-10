﻿using System.Reflection;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Common;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Infrastructure.Persistence.SqlServer;

public class AppDbContext:DbContext,IAppDbContext
{
    public AppDbContext(DbContextOptions options):base(options)
    {
        
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDateTime =DateTime.Now;
                    entry.Entity.CreatedUserId =1;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedDateTime = DateTime.Now;
                    entry.Entity.ModifiedUserId =1;
                    break;
            }
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }
}