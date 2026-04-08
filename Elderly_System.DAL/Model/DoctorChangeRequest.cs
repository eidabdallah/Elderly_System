using Elderly_System.DAL.Enums;
using ElderlySystem.DAL.Model;
using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.Model
{
    public class DoctorChangeRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "المسن مطلوب.")]
        public int ElderlyId { get; set; }
        public Elderly Elderly { get; set; } = null!;

        [Required(ErrorMessage = "الدكتور المطلوب مطلوب.")]
        public string RequestedDoctorId { get; set; } = null!;
        public Doctor RequestedDoctor { get; set; } = null!;
        public Status RequestStatus { get; set; } = Status.Pending;
    }
}
