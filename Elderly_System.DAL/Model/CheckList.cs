using ElderlySystem.DAL.Model;
using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.Model
{
    public class CheckList
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "المسن مطلوب.")]
        public int ElderlyId { get; set; }
        public Elderly Elderly { get; set; } = null!;

        [Required(ErrorMessage = "الممرضة مطلوبة.")]
        public string NurseId { get; set; } = null!;
        public Nurse Nurse { get; set; } = null!;
        public DateTime DateTime { get; set; } = DateTime.Now;
        [StringLength(500, ErrorMessage = "الملاحظات يجب ألا تتجاوز 500 حرف.")]
        [Required(ErrorMessage = "الملاحظات مطلوبة.")]
        public string Notes { get; set; } = null!;

        [Required(ErrorMessage = "درجة الحرارة مطلوبة.")]
        public string Temperature { get; set; } = null!;

        [Required(ErrorMessage = "النبض مطلوب.")]
        public string Pulse { get; set; } = null!;

        [Required(ErrorMessage = "سكر الدم مطلوب.")]
        public string BloodSugar { get; set; } = null!;

        [StringLength(20, ErrorMessage = "ضغط الدم يجب ألا يتجاوز 20 حرف.")]
        [Required(ErrorMessage = "ضغط الدم مطلوب.")]
        public string BloodPressure { get; set; } = null!;

        [Required(ErrorMessage = "المدخول مطلوب.")]
        public string Intake { get; set; } = null!;

        [Required(ErrorMessage = "الإخراج مطلوب.")]
        public string Output { get; set; } = null!;

        [Required(ErrorMessage = "الفرق بين المدخول والإخراج مطلوب.")]
        public string InOut { get; set; } = null!;

    }
}
