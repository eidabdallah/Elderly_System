using System.ComponentModel.DataAnnotations;

namespace ElderlySystem.DAL.Model
{
    public class Good
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "الاسم مطلوب.")]
        public string NameGood { get; set; } = null!;

        [Required(ErrorMessage = "الاسم مطلوب.")]
        [Range(1, int.MaxValue, ErrorMessage = "الكمية يجب أن تكون رقمًا أكبر من صفر.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "التبرع مطلوب.")]

        public int DonationId { get; set; }
        public Donation Donation { get; set; } = null!;
    }
}
