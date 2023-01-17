using OnlineShop.Application.Common;

namespace OnlineShop.Application.ApplicationServices.ProductServices.Responses;

public class ProductDto:AuditableDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public long Quantity { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    
}