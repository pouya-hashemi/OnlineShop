using OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

namespace OnlineShop.Domain.DomainServices;

public class TokenManager:ITokenManager
{
    public TokenManager()
    {
        
    }
    public Task<string> GenerateGuestToken()
    {
        throw new NotImplementedException();
    }

    public Task<string> GenerateVendorToken()
    {
        throw new NotImplementedException();
    }
}