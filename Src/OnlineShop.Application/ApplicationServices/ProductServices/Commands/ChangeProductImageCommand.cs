using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Application.ApplicationServices.ProductServices.Commands;

public class ChangeProductImageCommand:IRequest
{
    public long ProductId { get; set; }
    public IFormFile ImageFile { get; set; }
}
public class ChangeProductImageHandler:IRequestHandler<ChangeProductImageCommand,Unit>
{
    private readonly IAppDbContext _context;
    private readonly IProductManager _productManager;
    private readonly IFileService _fileService;

    public ChangeProductImageHandler(IAppDbContext context,
        IProductManager productManager,
        IFileService fileService)
    {
        _context = context;
        _productManager = productManager;
        _fileService = fileService;
    }
    public async Task<Unit> Handle(ChangeProductImageCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Where(w => w.Id == request.ProductId)
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(nameof(product));
        }

        var imagePath =await _fileService.SaveImageFile(request.ImageFile);

        _fileService.RemoveFile(product.ImageUrl);
        
        _productManager.ChangeImageUrl(product,imagePath);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}