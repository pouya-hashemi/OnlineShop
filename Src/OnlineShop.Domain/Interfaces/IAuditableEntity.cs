namespace OnlineShop.Domain.Interfaces;

public interface IAuditableEntity
{
   long CreatedUserId { get; set; }
   DateTime  CreatedDateTime { get; set; }
   long? ModifiedUserId { get; set; }
   DateTime? ModifiedDateTime { get; set; }
}