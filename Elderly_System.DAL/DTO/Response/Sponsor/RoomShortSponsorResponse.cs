using Elderly_System.DAL.DTO.Response.Room;
using Elderly_System.DAL.Enums;
using System.Text.Json.Serialization;

namespace Elderly_System.DAL.DTO.Response.Sponsor
{
    public class RoomShortSponsorResponse
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
        public List<RoomImageResponse> Images { get; set; } = new();
    }
}
