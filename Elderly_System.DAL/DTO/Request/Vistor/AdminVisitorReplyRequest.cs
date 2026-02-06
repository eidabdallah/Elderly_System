using Elderly_System.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.DTO.Request.Vistor
{
    public class AdminVisitorReplyRequest
    {
        [Required(ErrorMessage = "الحالة مطلوبة.")]
        public Status Status { get; set; }

        [Required(ErrorMessage = "رسالة الرد مطلوبة.")]
        public string Message { get; set; } = null!;
    }
}
