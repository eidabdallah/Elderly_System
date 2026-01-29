
using ElderlySystem.DAL.Model;
using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.Model
{
    public class Medication
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "الخطة الدوائية مطلوبة.")]
        public int DrugPlanId { get; set; }
        public DrugPlan DrugPlan { get; set; } = null!;

        [Required(ErrorMessage = "الممرضة مطلوبة.")]
        public string NurseId { get; set; } = null!;
        public Nurse Nurse { get; set; } = null!;

        [Required(ErrorMessage = "التاريخ والوقت مطلوب.")]
        public DateTime DateTime { get; set; }

        [Required(ErrorMessage = "الجرعة مطلوبة.")]
        [StringLength(100, ErrorMessage = "الجرعة يجب ألا تتجاوز 100 حرف.")]
        public string Dose { get; set; } = null!;
    }
}
