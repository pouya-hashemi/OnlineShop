using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.ApplicationServices.ProductServices.Responses;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Application.ApplicationServices.ProductServices.Commands;

public class CreateProductCommand : IRequest<ProductDto>
{
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int CategoryId { get; set; }
    public IFormFile ImageFile { get; set; }
}

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IAppDbContext _context;
    private readonly IProductManager _productManager;
    private readonly IFileService _fileService;

    public CreateProductHandler(IAppDbContext context,
        IProductManager productManager,
        IFileService fileService)
    {
        _context = context;
        _productManager = productManager;
        _fileService = fileService;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var imageUrl = await _fileService.SaveImageFile(request.ImageFile);
        
        var category = await _context.Categories
            .Where(w => w.Id == request.CategoryId)
            .FirstOrDefaultAsync(cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(nameof(category));
        }

        var product =
            await _productManager.CreateProductAsync(request.ProductName, imageUrl, request.Price, request.Quantity, category,cancellationToken);

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        var productDto = product.Adapt<ProductDto>();

        return productDto;
    }
}