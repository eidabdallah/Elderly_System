using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.DTO.Request.Elderly
{
    public class CreateDoctorChangeRequestDto
    {
        [Required(ErrorMessage = "معرّف الدكتور المطلوب مطلوب.")]
        public string RequestedDoctorId { get; set; } = null!;
    }
}
