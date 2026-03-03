using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.DTO.Request.Medicine
{
    public class DrugPlanUpdateRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Range(1, 20, ErrorMessage = "عدد الجرعات اليومية يجب أن يكون بين 1 و 20.")]
        public int? DailyIntake { get; set; }

        [StringLength(500, ErrorMessage = "الملاحظات يجب ألا تتجاوز 500 حرف.")]
        public string? Notes { get; set; }

        public List<TimeSpan>? Times { get; set; }
    }
}

