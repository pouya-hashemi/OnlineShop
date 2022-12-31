using System.Security.Cryptography;
using System.Text;

namespace OnlineShop.Domain.DomainServices;

public class HashManager
{
    public string CreateHash(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException($"'{nameof(text)}' cannot be null or whitespace.", nameof(CreateHash));
        }

        using (SHA256 mySha256 = SHA256.Create())
        {
            var hash = mySha256.ComputeHash(Encoding.ASCII.GetBytes(text));
            var hashStr = Encoding.ASCII.GetString(hash);
            return hashStr;
        }
    }
}