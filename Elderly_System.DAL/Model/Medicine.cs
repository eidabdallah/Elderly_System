using Elderly_System.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.Model
{
    public class Medicine
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الدواء مطلوب.")]
        [StringLength(150, ErrorMessage = "اسم الدواء يجب ألا يتجاوز 150 حرف.")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "نوع الدواء مطلوب.")]
        public MedicineType Type { get; set; }
        public ICollection<DrugPlan> DrugPlans { get; set; } = new List<DrugPlan>();
    }
}
