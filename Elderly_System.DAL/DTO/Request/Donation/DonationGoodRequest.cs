using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.DTO.Request.Donation
{
    public class DonationGoodRequest
    {
        [Required(ErrorMessage = "الاسم مطلوب.")]
        public string NameGood { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "الكمية يجب أن تكون رقمًا أكبر من صفر.")]
        public int Quantity { get; set; }
    }
}
