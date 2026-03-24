using CTPS.API.Common;
using CTPS.API.DTOs.Auth;
using CTPS.API.Helpers;
using CTPS.API.Models;
using CTPS.API.Repositories.Interfaces;
using CTPS.API.Services.Interfaces;

namespace CTPS.API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenService tokenService;

        public AuthService(IUserRepository _userRepository, ITokenService _tokenService)
        {
            userRepository = _userRepository;
            tokenService = _tokenService;
        }

        public async Task<Result<AuthResponseDTO>> Register(RegisterRequestDTO request)
        {
            string normalizedEmail = request.Email.Trim().ToLowerInvariant();
            string normalizedMobile = request.Mobile.Trim();

            if (await userRepository.GetUserByEmail(normalizedEmail) != null)
            {
                return Result<AuthResponseDTO>.Fail(409, ["An account with this email already exists."]);
            }

            if (await userRepository.GetUserByMobile(normalizedMobile) != null)
            {
                return Result<AuthResponseDTO>.Fail(409, ["An account with this mobile number already exists."]);
            }

            var user = new User
            {
                Name = request.Name.Trim(),
                Email = normalizedEmail,
                Mobile = normalizedMobile,
                PasswordHash = PasswordHasher.HashPassword(request.Password),
                Role = "Commuter",
                CreatedAt = DateTime.Now
            };

            User createdUser = await userRepository.CreateUser(user);

            return Result<AuthResponseDTO>.Ok(new AuthResponseDTO
            {
                Token = tokenService.GenerateToken(createdUser),
                User = MapUser(createdUser)
            }, 201);
        }

        public async Task<Result<AuthResponseDTO>> Login(LoginRequestDTO request)
        {
            string normalizedEmail = request.Email.Trim().ToLowerInvariant();
            User? user = await userRepository.GetUserByEmail(normalizedEmail);

            if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Result<AuthResponseDTO>.Fail(401, ["Invalid email or password."]);
            }

            return Result<AuthResponseDTO>.Ok(new AuthResponseDTO
            {
                Token = tokenService.GenerateToken(user),
                User = MapUser(user)
            }, 200);
        }

        public async Task<Result<UserProfileDTO>> GetCurrentUser(int userId)
        {
            User? user = await userRepository.GetUserById(userId);
            if (user == null)
            {
                return Result<UserProfileDTO>.Fail(404, ["User not found."]);
            }

            return Result<UserProfileDTO>.Ok(MapUser(user), 200);
        }

        private static UserProfileDTO MapUser(User user)
        {
            return new UserProfileDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email ?? string.Empty,
                Mobile = user.Mobile,
                Role = user.Role
            };
        }
    }
}
