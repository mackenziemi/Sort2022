using Sort2022.Models;

namespace Sort2022.Interfaces
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, string audience, User user);
    }
}
