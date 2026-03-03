using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elderly_System.DAL.DTO.Request.Medicine
{
    public class AddDrugPlanRequest
    {
        [Required(ErrorMessage = "المسن مطلوب.")]
        public int ElderlyId { get; set; }

        public int? MedicineId { get; set; }

        public NewMedicineDto? NewMedicine { get; set; }

        [Required(ErrorMessage = "تاريخ البداية مطلوب.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "تاريخ النهاية مطلوب.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "عدد الجرعات اليومية مطلوب.")]
        [Range(1, 20, ErrorMessage = "عدد الجرعات اليومية يجب أن يكون بين 1 و 20.")]
        public int DailyIntake { get; set; }

        [StringLength(500, ErrorMessage = "الملاحظات يجب ألا تتجاوز 500 حرف.")]
        public string? Notes { get; set; }

        [Required(ErrorMessage = "مواعيد الجرعات مطلوبة.")]
        [MinLength(1, ErrorMessage = "يجب إدخال وقت جرعة واحد على الأقل.")]
        public List<TimeSpan> Times { get; set; } = new();
    }
}
