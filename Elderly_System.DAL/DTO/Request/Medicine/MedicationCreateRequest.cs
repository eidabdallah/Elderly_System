using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.DTO.Request.Medicine
{
    public class MedicationCreateRequest
    {
        [Required(ErrorMessage = "المسن مطلوب.")]
        public int ElderlyId { get; set; }

        [Required(ErrorMessage = "الخطة الدوائية مطلوبة.")]
        public int DrugPlanId { get; set; }

        [Required(ErrorMessage = "الجرعة مطلوبة.")]
        [StringLength(100, ErrorMessage = "الجرعة يجب ألا تتجاوز 100 حرف.")]
        public string Dose { get; set; } = null!;
    }
}
