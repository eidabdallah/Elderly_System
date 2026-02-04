using Elderly_System.DAL.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Elderly_System.DAL.DTO.Request.Room
{
    public class RoomStatusUpdateRequest
    {
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]

        public Status Status { get; set; }
    }
}
