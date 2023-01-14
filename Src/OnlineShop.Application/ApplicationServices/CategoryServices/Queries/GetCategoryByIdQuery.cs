using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.ApplicationServices.CategoryServices.Responses;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Application.ApplicationServices.CategoryServices.Queries;

public class GetCategoryByIdQuery : IRequest<CategoryDto>
{
    public int CategoryId { get; set; }
}

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{
    private readonly IAppDbContext _context;

    public GetCategoryByIdHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .Where(w => w.Id == request.CategoryId)
            .Select(s => new CategoryDto()
            {
                Id = s.Id,
                Name = s.Name,
                CreatedDateTime = s.CreatedDateTime,
                CreatedUserId = s.CreatedUserId,
                ModifiedDateTime = s.ModifiedDateTime,
                ModifiedUserId = s.ModifiedUserId
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(nameof(category));
        }

        return category;
    }
}