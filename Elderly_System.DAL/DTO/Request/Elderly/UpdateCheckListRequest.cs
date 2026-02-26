using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.DTO.Request.Elderly
{
    public class UpdateCheckListRequest
    {
        [Required(ErrorMessage = "الملاحظات مطلوبة.")]
        [StringLength(500, ErrorMessage = "الملاحظات يجب ألا تتجاوز 500 حرف.")]
        public string Notes { get; set; } = null!;

        [Required(ErrorMessage = "درجة الحرارة مطلوبة.")]
        public string Temperature { get; set; } = null!;

        [Required(ErrorMessage = "النبض مطلوب.")]
        public string Pulse { get; set; } = null!;

        [Required(ErrorMessage = "سكر الدم مطلوب.")]
        public string BloodSugar { get; set; } = null!;

        [Required(ErrorMessage = "ضغط الدم مطلوب.")]
        [StringLength(20, ErrorMessage = "ضغط الدم يجب ألا يتجاوز 20 حرف.")]
        public string BloodPressure { get; set; } = null!;

        [Required(ErrorMessage = "المدخول مطلوب.")]
        public string Intake { get; set; } = null!;

        [Required(ErrorMessage = "الإخراج مطلوب.")]
        public string Output { get; set; } = null!;

        [Required(ErrorMessage = "الفرق بين المدخول والإخراج مطلوب.")]
        public string InOut { get; set; } = null!;
    }
}
