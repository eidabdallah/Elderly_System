using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.Model
{
    public class Medicine
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الدواء مطلوب.")]
        [StringLength(150, ErrorMessage = "اسم الدواء يجب ألا يتجاوز 150 حرف.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "وصف الدواء مطلوب.")]
        [StringLength(500, ErrorMessage = "الوصف يجب ألا يتجاوز 500 حرف.")]
        public string Description { get; set; } = null!;

        //public ICollection<DrugPlan> DrugPlans { get; set; } = new List<DrugPlan>();
        public ICollection<MedicalReportMedicine> MedicalReportMedicines { get; set; } = new List<MedicalReportMedicine>();
    }
}
