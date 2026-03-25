using ElderlySystem.DAL.Model;
using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.Model
{
    public class MedicalReport
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "التاريخ مطلوب.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "ملف التشخيص مطلوب.")]
        public string DiagnosisUrl { get; set; } = null!;
        [Required]
        public string DiagnosisPublicId { get; set; } = null!;

        [Required(ErrorMessage = "المسن مطلوب.")]
        public int ElderlyId { get; set; }
        public Elderly Elderly { get; set; } = null!;

        [Required(ErrorMessage = "الطبيب مطلوب.")]
        public string DoctorId { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;

    }
}
