using CTPS.API.Common;
using CTPS.API.DTOs.Auth;

namespace CTPS.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthResponseDTO>> Register(RegisterRequestDTO request);
        Task<Result<AuthResponseDTO>> Login(LoginRequestDTO request);
        Task<Result<UserProfileDTO>> GetCurrentUser(int userId);
    }
}
