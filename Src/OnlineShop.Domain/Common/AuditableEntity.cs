namespace OnlineShop.Domain.Common;

public class AuditableEntity
{
    public long CreatedUserId { get; set; }
    public DateTime  CreatedDateTime { get; set; }
    public long? ModifiedUserId { get; set; }
    public DateTime? ModifiedDateTime { get; set; }
}