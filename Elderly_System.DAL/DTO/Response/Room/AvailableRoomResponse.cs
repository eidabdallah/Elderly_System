using Elderly_System.DAL.Enums;
using System.Text.Json.Serialization;

namespace Elderly_System.DAL.DTO.Response.Room
{
    public class AvailableRoomResponse
    {
        public int RoomId { get; set; }
        public int RoomNumber { get; set; }
        [JsonIgnore]
        public RoomType RoomType { get; set; }
        public string RoomTypeName => RoomType switch
        {
            RoomType.FirstClass => "درجة أولى",
            RoomType.SecondClass => "درجة ثانية",
            _ => "غير معروف"
        };
    }
}
