using Elderly_System.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.DTO.Request.User
{
    public class ChangeUserStatusRequest
    {
        [Required]
        public Status Status { get; set; }
    }
}
