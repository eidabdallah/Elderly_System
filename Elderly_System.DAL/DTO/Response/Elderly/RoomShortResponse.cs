using Elderly_System.DAL.Enums;
using System.Text.Json.Serialization;

namespace Elderly_System.DAL.DTO.Response.Elderly
{
    public class RoomShortResponse
    {
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
