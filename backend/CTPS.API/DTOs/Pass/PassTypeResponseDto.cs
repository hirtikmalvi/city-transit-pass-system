namespace CTPS.API.DTOs.Pass
{
    public class PassTypeResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int ValidityDays { get; set; }
        public decimal Price { get; set; }
        public int? MaxTripsPerDay { get; set; }
        public List<string> TransportModes { get; set; } = new();
    }
}
