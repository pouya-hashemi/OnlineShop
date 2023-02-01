namespace OnlineShop.Domain.Interfaces.DomainServiceInterfaces;

public interface ITokenManager
{
    Task<string> GenerateGuestToken();
    Task<string> GenerateVendorToken();
}