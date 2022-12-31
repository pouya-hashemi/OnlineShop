using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Domain.DomainServices;

public class UserManager
{
    private readonly IAppDbContext _dbContext;

    public UserManager(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<User> CreateUser(string username, string password, string userTitle)
    {
        if (await UsernameExistAsync(username))
        {
            throw new AlreadyExistException(nameof(username), username);
        }
        
        return new User(username, password, userTitle);
    }


    #region Validate

    private async Task<bool> UsernameExistAsync(string username, long? userId=null,CancellationToken cancellationToken=default)
    {
        var query = _dbContext.Users
            .Where(w => w.Username == username)
            .AsQueryable();
        if (userId!=null)
        {
            query = query
                .Where(w => w.Id != userId);
        }

        return await query.AnyAsync(cancellationToken);
    }

    #endregion

    #region Setter

    public async Task ChangeUsernameAsync(User user, string username)
    {
        if (await UsernameExistAsync(username,user.Id))
        {
            throw new AlreadyExistException(nameof(username), username);
        }
        
        user.SetUsername(username);
    }

    public void ChangeUserTitle(User user, string userTitle)
    {
        user.SetUserTitle(userTitle);
    }

    #endregion
}