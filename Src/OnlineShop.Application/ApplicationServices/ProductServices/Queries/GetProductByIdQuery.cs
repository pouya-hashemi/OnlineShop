using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineShop.Application.ApplicationServices.ProductServices.Responses;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Application.ApplicationServices.ProductServices.Queries;

public class GetProductByIdQuery:IRequest<ProductDto>
{
    public long ProductId { get; set; }
}
public class GetProductByIdHandler:IRequestHandler<GetProductByIdQuery,ProductDto>
{
    private readonly IAppDbContext _context;
    private readonly IConfiguration _configuration;

    public GetProductByIdHandler(IAppDbContext context,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product =await _context.Products
            .Include(i => i.Category)
            .Where(w=>w.Id==request.ProductId)
            .Select(s => new ProductDto()
            {
                Id = s.Id,
                Name = s.Name,
                Price = s.Price,
                Quantity = s.Quantity,
                CategoryId = s.CategoryId,
                CategoryName = s.Category.Name,
                ImageUrl =s.ImageUrl,
                CreatedDateTime = s.CreatedDateTime,
                CreatedUserId = s.CreatedUserId,
                ModifiedDateTime = s.ModifiedDateTime,
                ModifiedUserId = s.ModifiedUserId
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(nameof(product));
        }

        return product;


    }
}