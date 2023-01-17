using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.EntityPropertyConfigurations;

namespace OnlineShop.Infrastructure.Persistence.SqlServer.EntityConfigurations;

public class CategoryEntityConfiguration:IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .Property(p => p.Name)
            .HasMaxLength(CategoryPropertyConfiguration.NameMaxLength)
            .IsRequired();
    }
}