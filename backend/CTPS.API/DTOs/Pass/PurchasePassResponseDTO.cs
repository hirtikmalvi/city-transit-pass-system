namespace CTPS.API.DTOs.Pass
{
    public class PurchasePassResponseDTO
    {
        public int UserPassId { get; set; }
        public string PassCode { get; set; } = null!;
        public string PassTypeName { get; set; } = null!;
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; } = null!;
        public List<string> CoveredTransportModes { get; set; } = new();
    }
}
