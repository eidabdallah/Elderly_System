using Elderly_System.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace ElderlySystem.DAL.Model
{ 
    public class ResidentStay
    {
        public int Id { get; set; }
        public Status Status { get; set; } = Status.Active;
        [Required(ErrorMessage = "تاريخ البداية مطلوب.")]
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime? EndDate { get; set; }
        public int ElderlyId { get; set; }
        public Elderly Elderly { get; set; } = null!;

        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
    }
}
