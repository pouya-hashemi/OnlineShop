using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Application.ApplicationServices.ProductServices.Commands;

public class UpdateProductCommand:IRequest
{
    public long ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int  CategoryId { get; set; }
}
public class UpdateProductHandler:IRequestHandler<UpdateProductCommand,Unit>
{
    private readonly IAppDbContext _context;
    private readonly IProductManager _productManager;

    public UpdateProductHandler(IAppDbContext context,
        IProductManager productManager)
    {
        _context = context;
        _productManager = productManager;
    }
    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(i=>i.Category)
            .Where(w=>w.Id==request.ProductId)
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(nameof(product));
        }

        var category = await _context.Categories

            .Where(w => w.Id == request.CategoryId)
            .FirstOrDefaultAsync(cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(nameof(category));
        }
        
        await _productManager.ChangeNameAsync(product, request.ProductName,cancellationToken);
         _productManager.ChangeCategory(product, category);
         _productManager.ChangePrice(product,request.Price);
         _productManager.ChangeQuantity(product,request.Quantity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}