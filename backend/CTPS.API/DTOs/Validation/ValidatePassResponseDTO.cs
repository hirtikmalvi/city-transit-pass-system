namespace CTPS.API.DTOs.Validation
{
    public class ValidatePassResponseDTO
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = null!;
        public string? PassHolderName { get; set; }
        public string? PassTypeName { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? TripsUsedToday { get; set; }
        public int? MaxTripsPerDay { get; set; }
    }
}
