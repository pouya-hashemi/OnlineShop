using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Domain.Common;

public class AuditableEntity:IAuditableEntity
{
    public long CreatedUserId { get; set; }
    public DateTime  CreatedDateTime { get; set; }
    public long? ModifiedUserId { get; set; }
    public DateTime? ModifiedDateTime { get; set; }
}