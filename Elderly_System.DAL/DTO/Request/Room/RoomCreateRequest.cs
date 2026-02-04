using ElderlySystem.DAL.Enums;
using Microsoft.AspNetCore.Http;

namespace Elderly_System.DAL.DTO.Request.Room
{
    public class RoomCreateRequest
    {
        public int RoomNumber { get; set; }
        public RoomType RoomType { get; set; }
        public int Capacity { get; set; }
        public float Price { get; set; }
        public string Description { get; set; } = null!;
        public List<IFormFile> Images { get; set; } = new();
    }
}
