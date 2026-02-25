using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.DTO.Request.Elderly
{
    public class UploadComprehensiveExamRequest
    {
        [Required(ErrorMessage = "صورة الفحص الشامل مطلوبة.")]
        public IFormFile File { get; set; } = null!;
    }
}
