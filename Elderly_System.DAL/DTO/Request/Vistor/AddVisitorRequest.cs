using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.DTO.Request.Vistor
{
    public class AddVisitorRequest
    {
        [Required(ErrorMessage = "الاسم مطلوب.")]
        [MinLength(3, ErrorMessage = "الاسم يجب أن يحتوي على 3 أحرف على الأقل.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "رقم الهاتف مطلوب.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "رقم الهاتف يجب أن يتكون من 10 أرقام.")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "تاريخ الزيارة مطلوب.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "وقت البداية مطلوب.")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "وقت النهاية مطلوب.")]
        public TimeSpan EndTime { get; set; }
    }
}
