using OnlineShop.Application.Common;

namespace OnlineShop.Application.ApplicationServices.CategoryServices.Responses;

public class CategoryDto:AuditableDto
{
    public int Id{ get; set; }
    public string Name { get; set; }
}