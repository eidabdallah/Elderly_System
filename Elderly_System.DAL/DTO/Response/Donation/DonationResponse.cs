using System.Text.Json.Serialization;

namespace Elderly_System.DAL.DTO.Response.Donation
{
    public class DonationResponse
    {
        public int Id { get; set; }
        public string DonorName { get; set; } = null!;
        public string DonationDate { get; set; } = null!;
        public string DonationType { get; set; } = null!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? MonetaryAmount { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Currency { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<GoodResponse>? Goods { get; set; }
    }
}
