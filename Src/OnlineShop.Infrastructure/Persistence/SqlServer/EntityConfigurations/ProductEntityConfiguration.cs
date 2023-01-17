using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.EntityPropertyConfigurations;

namespace OnlineShop.Infrastructure.Persistence.SqlServer.EntityConfigurations;

public class ProductEntityConfiguration:IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .Property(p => p.Name)
            .HasMaxLength(ProductPropertyConfiguration.NameMaxLength)
            .IsRequired();

        builder
            .Property(p => p.ImageUrl)
            .HasMaxLength(ProductPropertyConfiguration.ImageUrlMaxLength)
            .IsRequired();

        builder
            .Property(p => p.Price)
            .HasColumnType("decimal(18,3)");

        builder
            .ToTable(t => t.HasCheckConstraint("Chk_Price_MinValue", $"Price >= {ProductPropertyConfiguration.PriceMinValue}"));
        builder
            .ToTable(t => t.HasCheckConstraint("Chk_Quantity_MinValue", $"Quantity >= {ProductPropertyConfiguration.QuantityMinValue}"));

        builder
            .HasOne(a => a.Category)
            .WithMany(w => w.Products)
            .HasForeignKey(f => f.CategoryId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}