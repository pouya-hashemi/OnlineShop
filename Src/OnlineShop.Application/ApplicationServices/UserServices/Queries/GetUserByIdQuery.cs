using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.ApplicationServices.UserServices.Responses;
using OnlineShop.Domain.Exceptions.BaseExceptions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Application.ApplicationServices.UserServices.Queries;

public class GetUserByIdQuery:IRequest<UserDto>
{
    public long UserId { get; set; }
}
public class GetUserByIdHandler:IRequestHandler<GetUserByIdQuery,UserDto>
{
    private readonly IAppDbContext _context;

    public GetUserByIdHandler(IAppDbContext context)
    {
        _context = context;
    }
    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user =await _context.Users
            .Where(w=>w.Id==request.UserId)
            .Select(s=>new UserDto()
            {
                Id = s.Id,
                Username = s.UserName,
                UserTitle = s.UserTitle,
                
                CreatedDateTime = s.CreatedDateTime,
                CreatedUserId = s.CreatedUserId,
                ModifiedDateTime = s.ModifiedDateTime,
                ModifiedUserId = s.ModifiedUserId
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(nameof(user));
        }

        return user;

    }
}