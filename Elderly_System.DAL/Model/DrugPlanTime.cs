using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.Model
{
    public class DrugPlanTime
    {
        public int Id { get; set; }

        public int DrugPlanId { get; set; }
        public DrugPlan DrugPlan { get; set; } = null!;

        [Required(ErrorMessage = "وقت الجرعة مطلوب.")]
        public TimeSpan Time { get; set; }
    }
}
