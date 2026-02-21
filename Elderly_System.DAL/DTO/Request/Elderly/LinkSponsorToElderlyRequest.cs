using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.DTO.Request.Elderly
{
    public class LinkSponsorToElderlyRequest
    {
        [Required]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "يجب أن يكون رقم هوية المسن مكوّنًا من 9 أرقام.")]
        public string ElderlyNationalId { get; set; } = null!;

        [Required(ErrorMessage = "صلة القرابة مطلوبة.")]
        public string KinShip { get; set; } = null!;

        [Required(ErrorMessage = "درجة القرابة مطلوبة.")]
        public string Degree { get; set; } = null!;
    }
}
