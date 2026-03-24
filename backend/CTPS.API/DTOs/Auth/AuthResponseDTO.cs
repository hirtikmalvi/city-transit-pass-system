namespace CTPS.API.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public UserProfileDTO User { get; set; } = new();
    }
}
