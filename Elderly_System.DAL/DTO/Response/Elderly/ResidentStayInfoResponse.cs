using Elderly_System.DAL.Enums;

namespace Elderly_System.DAL.DTO.Response.Elderly
{
    public class ResidentStayInfoResponse
    {
        public int StayId { get; set; }
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public string Status { get; set; } = null!;
        public RoomShortResponse Room { get; set; } = null!;
    }
}
