using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ElderlySystem.DAL.Model
{
    [Index(nameof(NurseId), nameof(Date), IsUnique = true)]
    public class NurseShiftAssignment
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "رقم الممرضة مطلوب.")]
        public string NurseId { get; set; } = null!;
        public Nurse Nurse { get; set; } = null!;
        [Required(ErrorMessage = "الشفت مطلوب.")]
        public int ShiftId { get; set; }
        public Shift Shift { get; set; } = null!;
        [Required(ErrorMessage = "التاريخ مطلوب.")]
        public DateTime Date { get; set; }

    }
}
