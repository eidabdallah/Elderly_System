using ElderlySystem.DAL.Model;
using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.Model
{
    public class DrugPlan
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "المسن مطلوب.")]
        public int ElderlyId { get; set; }
        public Elderly Elderly { get; set; } = null!;

        [Required(ErrorMessage = "الدواء مطلوب.")]
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; } = null!;

        [Required(ErrorMessage = "تاريخ البداية مطلوب.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "تاريخ النهاية مطلوب.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "عدد الجرعات اليومية مطلوب.")]
        [Range(1, 20, ErrorMessage = "عدد الجرعات اليومية يجب أن يكون بين 1 و 20.")]
        public int DailyIntake { get; set; }

        [StringLength(500, ErrorMessage = "الملاحظات يجب ألا تتجاوز 500 حرف.")]
        public string? Notes { get; set; }

        public ICollection<Medication> Medications { get; set; } = new List<Medication>();
    }
}
