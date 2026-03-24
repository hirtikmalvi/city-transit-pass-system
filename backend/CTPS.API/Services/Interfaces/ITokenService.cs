using CTPS.API.Models;

namespace CTPS.API.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
