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
        public string? Notes { get; set; }
        public string? Temperature { get; set; }
        public string? Pulse { get; set; }
        public string? BloodSugar { get; set; }
        public string? BloodPressure { get; set; }
        public string? Intake { get; set; }
        public string? Output { get; set; }
    }
}
