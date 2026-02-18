using Elderly_System.DAL.DTO.Request.Room;

namespace Elderly_System.DAL.DTO.Response.Room
{
    public class RoomDetailsResponse : RoomResponse
    {
        public List<RoomImageResponse> Images { get; set; } = new();
        public List<RoomElderlyResponse> CurrentElderlies { get; set; } = new();
    }
}
