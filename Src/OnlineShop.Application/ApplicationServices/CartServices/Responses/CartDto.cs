using OnlineShop.Application.Common;

namespace OnlineShop.Application.ApplicationServices.CartServices.Responses;

public class CartDto:AuditableDto
{
    public long Id { get; set; }
    public string ProductName { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
}