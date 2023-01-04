namespace OnlineShop.Domain.Common;

public class EntityBase<T>:AuditableEntity
{
    public T Id { get; set; }

}