using Elderly_System.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace ElderlySystem.DAL.Model
{
    
    public class ElderlyVisitor
    {
        public int Id { get; set; }
        public int ElderlyId { get; set; }
        public Elderly Elderly { get; set; } = null!;
        public int VisitorId { get; set; }
        public Visitor Visitor { get; set; } = null!;
        [Required(ErrorMessage = "تاريخ الزيارة مطلوب.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "وقت بداية الزيارة مطلوب.")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "وقت نهاية الزيارة مطلوب.")]
        public TimeSpan EndTime { get; set; }
        public Status Status { get; set; } = Status.Pending;

    }
}
