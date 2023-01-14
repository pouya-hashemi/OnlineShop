using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Application.ApplicationServices.CategoryServices.Commands;

public class DeleteCategoryCommand:IRequest
{
    public int CategoryId { get; set; }
}

public class DeleteCategoryHandler:IRequestHandler<DeleteCategoryCommand,Unit>
{
    private readonly IAppDbContext _context;
    private readonly ICategoryManager _categoryManager;

    public DeleteCategoryHandler(IAppDbContext context,
        ICategoryManager categoryManager)
    {
        _context = context;
        _categoryManager = categoryManager;
    }
    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(f => f.Id == request.CategoryId, cancellationToken);
        if (category is null)
        {
            throw new NotFoundException(nameof(category));
        }

        await _categoryManager.IsDeletable(category);

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}