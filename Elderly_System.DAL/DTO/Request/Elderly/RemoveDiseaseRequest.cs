using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.DTO.Request.Elderly
{
    public class RemoveDiseaseRequest
    {
        [Required(ErrorMessage = "اسم المرض مطلوب.")]
        public string Disease { get; set; } = null!;
    }
}
