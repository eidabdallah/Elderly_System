using Elderly_System.DAL.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Elderly_System.DAL.DTO.Request.Donation
{
    public class DonationCreateRequest
    {
        [Required(ErrorMessage = "اسم المتبرع مطلوب.")]
        [StringLength(100, ErrorMessage = "اسم المتبرع يجب ألا يتجاوز 100 حرف.")]
        public string DonorName { get; set; } = null!;
        public DateTime? DonationDate { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required(ErrorMessage = "نوع التبرع مطلوب.")]
        public DonationType DonationType { get; set; }
        public decimal? MonetaryAmount { get; set; }
        public string? Currency { get; set; }
        public List<DonationGoodRequest>? Goods { get; set; }
    }
}
