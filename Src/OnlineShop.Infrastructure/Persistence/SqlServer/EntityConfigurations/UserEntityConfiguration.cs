﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.EntityPropertyConfigurations;

namespace OnlineShop.Infrastructure.Persistence.SqlServer.EntityConfigurations;

public class UserEntityConfiguration:IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder
            .Property(p => p.UserName)
            .HasMaxLength(UserPropertyConfiguration.UsernameMaxLength);
 
        
        builder
            .Property(p => p.UserTitle)
            .HasMaxLength(UserPropertyConfiguration.UserTitleMaxLength);
        
        builder.HasIndex(p => p.UserName).IsUnique();
    }
}