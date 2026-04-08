using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.DTO.Request.Elderly
{
    public class AddMedicalReportDto
    {
        [Required(ErrorMessage = "المسن مطلوب.")]
        public int ElderlyId { get; set; }

        [Required(ErrorMessage = "ملف التشخيص مطلوب.")]
        public IFormFile DiagnosisFile { get; set; } = null!;
    }
}
