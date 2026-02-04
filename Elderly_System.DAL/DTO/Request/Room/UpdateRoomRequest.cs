using Elderly_System.DAL.Enums;
using System.Text.Json.Serialization;

namespace Elderly_System.DAL.DTO.Request.Room
{
    public class UpdateRoomRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RoomType? RoomType { get; set; }
        public int? Capacity { get; set; }
        public float? Price { get; set; }
        public string? Description { get; set; }
    }
}
