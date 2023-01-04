namespace OnlineShop.Application.Common;

public class AuditableDto
{
    public long CreatedUserId { get; set; }
    public DateTime  CreatedDateTime { get; set; }
    public long? ModifiedUserId { get; set; }
    public DateTime? ModifiedDateTime { get; set; }
}