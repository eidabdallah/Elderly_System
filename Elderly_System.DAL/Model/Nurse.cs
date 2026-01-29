using System.ComponentModel.DataAnnotations;

namespace ElderlySystem.DAL.Model
{
    public class Nurse : Employee
    {
        [Required(ErrorMessage = "الشهادة مطلوبة.")]
        public string ImageCertificate { get; set; } = null!;
        public ICollection<NurseShiftAssignment> NurseShiftAssignments { get; set; } = new List<NurseShiftAssignment>();
    }
}
