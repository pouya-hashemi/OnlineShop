using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

public interface ITokenManager
{
    string GenerateGuestToken();
    string GenerateVendorToken(User user);
}