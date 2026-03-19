namespace CTPS.API.DTOs.Trip
{
    public class TripResponseDTO
    {
        public int Id { get; set; }
        public string PassCode { get; set; } = null!;
        public string PassTypeName { get; set; } = null!;
        public string TransportMode { get; set; } = null!;
        public string? RouteInfo { get; set; }
        public DateTime? ValidatedAt { get; set; }
        public string? ValidatedByName { get; set; }
    }
}
