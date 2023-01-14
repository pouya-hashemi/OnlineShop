using Mapster;
using MediatR;
using OnlineShop.Application.ApplicationServices.CategoryServices.Responses;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Application.ApplicationServices.CategoryServices.Commands;

public class CreateCategoryCommand:IRequest<CategoryDto>
{
    public string CategoryName { get; set; }
}
public class CreateCategoryHandler:IRequestHandler<CreateCategoryCommand,CategoryDto>
{
    private readonly IAppDbContext _context;
    private readonly ICategoryManager _categoryManager;

    public CreateCategoryHandler(IAppDbContext context,
        ICategoryManager categoryManager)
    {
        _context = context;
        _categoryManager = categoryManager;
    }
    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category =await _categoryManager.CreateCategoryAsync(request.CategoryName, cancellationToken);

        _context.Categories.Add(category);

        await _context.SaveChangesAsync(cancellationToken);

        return category.Adapt<CategoryDto>();
    }
}