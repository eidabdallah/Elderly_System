namespace Elderly_System.DAL.DTO.Request.Donation
{
    public class DonationUpdateRequest
    {
        public decimal? MonetaryAmount { get; set; }
        public string? Currency { get; set; }
        public int? GoodId { get; set; }
        public string? NameGood { get; set; }
        public int? Quantity { get; set; }
    }
}
