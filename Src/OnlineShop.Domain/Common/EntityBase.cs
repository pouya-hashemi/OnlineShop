namespace OnlineShop.Domain.Common;

public class EntityBase<T>
{
    public T Id { get; set; }
    public long CreatedUserId { get; set; }
    public DateTime  CreatedDateTime { get; set; }
    public long? ModifiedUserId { get; set; }
    public DateTime? ModifiedDateTime { get; set; }
}