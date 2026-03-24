using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CTPS.API.Models;
using CTPS.API.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace CTPS.API.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public string GenerateToken(User user)
        {
            string key = configuration["Auth:JwtKey"]
                ?? throw new InvalidOperationException("JWT key is not configured.");

            string issuer = configuration["Auth:Issuer"] ?? "CTPS.API";
            string audience = configuration["Auth:Audience"] ?? "CTPS.Client";
            int expiryMinutes = int.TryParse(configuration["Auth:TokenExpiryMinutes"], out int parsedMinutes)
                ? parsedMinutes
                : 120;

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Email, user.Email ?? string.Empty),
                new(ClaimTypes.Role, user.Role)
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
