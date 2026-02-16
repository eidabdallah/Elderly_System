using Microsoft.AspNetCore.Http;

namespace Elderly_System.DAL.DTO.Request.Room
{
    public class ImageRoomRequest
    {
        public List<IFormFile>? Images { get; set; } = new();
        public List<string>? DeletedPublicIds { get; set; }
    }
}
