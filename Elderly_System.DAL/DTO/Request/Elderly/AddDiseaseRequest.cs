using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.DTO.Request.Elderly
{
    public class AddDiseasesRequest
    {
        [Required(ErrorMessage = "قائمة الأمراض مطلوبة.")]
        [MinLength(1, ErrorMessage = "يجب إرسال مرض واحد على الأقل.")]
        public List<string> Diseases { get; set; } = new();
    }
}
