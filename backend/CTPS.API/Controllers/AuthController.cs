using System.Security.Claims;
using CTPS.API.Common;
using CTPS.API.DTOs.Auth;
using CTPS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CTPS.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService _authService)
        {
            authService = _authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Result<AuthResponseDTO>>> Register([FromBody] RegisterRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(value => value.Errors)
                    .Select(error => string.IsNullOrWhiteSpace(error.ErrorMessage) ? "Invalid input." : error.ErrorMessage)
                    .ToList();

                return BadRequest(Result<AuthResponseDTO>.Fail(400, errors));
            }

            var result = await authService.Register(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Result<AuthResponseDTO>>> Login([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(value => value.Errors)
                    .Select(error => string.IsNullOrWhiteSpace(error.ErrorMessage) ? "Invalid input." : error.ErrorMessage)
                    .ToList();

                return BadRequest(Result<AuthResponseDTO>.Fail(400, errors));
            }

            var result = await authService.Login(request);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<Result<UserProfileDTO>>> Me()
        {
            string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(Result<UserProfileDTO>.Fail(401, ["Invalid authentication token."]));
            }

            var result = await authService.GetCurrentUser(userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
