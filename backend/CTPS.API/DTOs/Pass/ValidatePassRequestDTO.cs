namespace CTPS.API.DTOs.Pass
{
    public class ValidatePassRequestDTO
    {
        public string PassCode { get; set; } = null!;
        public string TransportModeCode { get; set; } = null!;
        public int ValidatedByUserId { get; set; }
        public string? RouteInfo { get; set; }
    }
}
