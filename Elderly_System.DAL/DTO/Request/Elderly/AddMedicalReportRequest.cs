using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.DTO.Request.Elderly
{
    public class AddMedicalReportRequest
    {
        [Required(ErrorMessage = "التاريخ مطلوب.")]
        public string Date { get; set; } = null!; 

        [Required(ErrorMessage = "ملف التشخيص مطلوب.")]
        public IFormFile DiagnosisFile { get; set; } = null!;
        public int? DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public string? DoctorWorkPlace { get; set; }
        public string? DoctorPhone { get; set; }
    }
}
