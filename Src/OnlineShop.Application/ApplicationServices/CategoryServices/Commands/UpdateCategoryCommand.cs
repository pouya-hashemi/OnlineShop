using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Application.ApplicationServices.CategoryServices.Commands;

public class UpdateCategoryCommand:IRequest
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
}
public  class UpdateCategoryHandler:IRequestHandler<UpdateCategoryCommand,Unit>
{
    private readonly IAppDbContext _context;
    private readonly ICategoryManager _categoryManager;

    public UpdateCategoryHandler(IAppDbContext context,
        ICategoryManager categoryManager)
    {
        _context = context;
        _categoryManager = categoryManager;
    }
    
    public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category =
            await _context.Categories.FirstOrDefaultAsync(f => f.Id == request.CategoryId, cancellationToken);
        if (category is null)
        {
            throw new NotFoundException(nameof(category));
        }

        await _categoryManager.ChangeNameAsync(category, request.CategoryName, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}