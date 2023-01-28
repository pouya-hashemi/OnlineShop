using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.EntityPropertyConfigurations;

namespace OnlineShop.Infrastructure.Persistence.SqlServer.EntityConfigurations;

public class CartEntityConfiguration:IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder
            .ToTable("Carts");
        
        builder
            .Property(p => p.Price)
            .HasColumnType("decimal(18,3)");

        builder
            .ToTable(t => t.HasCheckConstraint("Chk_Price_MinValue", $"Price >= {CartPropertyConfigurations.MinPrice}"));
        
        builder
            .ToTable(t => t.HasCheckConstraint("Chk_Quantity_MinValue", $"Quantity >= {CartPropertyConfigurations.MinQuantity}"));
    }
}