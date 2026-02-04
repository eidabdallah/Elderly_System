using Elderly_System.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace ElderlySystem.DAL.Model
{
    public class Room
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "رقم الغرفة مطلوب.")]
        [Range(1, int.MaxValue, ErrorMessage = "رقم الغرفة يجب أن يكون أكبر من صفر.")]
        public int RoomNumber { get; set; }
        [Required(ErrorMessage = "نوع الغرفة مطلوب.")]
        public RoomType RoomType { get; set; }
        [Required(ErrorMessage = "السعة للغرفة مطلوبة.")]
        [Range(1, int.MaxValue, ErrorMessage = "السعة يجب أن تكون واحد فأكثر.")]
        public int Capacity { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "السعة الحالية يجب أن تكون صفر فأكثر.")]
        public int CurrentCapacity { get; set; } = 0;
        [Required(ErrorMessage = "السعر مطلوب.")]
        [Range(1, double.MaxValue, ErrorMessage = "السعر يجب أن يكون رقمًا أكبر من صفر.")]
        public float Price { get; set; }
        [Required(ErrorMessage = "الوصف مطلوب.")]
        public string Description { get; set; } = null!;
        public Status Status { get; set; } = Status.Pending;
        public List<RoomImage> RoomImages { get; set; } = new List<RoomImage>();
        //relaion 
        public ICollection<ResidentStay> ResidentStays { get; set; } = new List<ResidentStay>();
    }
}
