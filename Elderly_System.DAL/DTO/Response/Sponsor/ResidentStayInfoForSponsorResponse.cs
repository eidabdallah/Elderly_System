namespace Elderly_System.DAL.DTO.Response.Sponsor
{
    public class ResidentStayInfoForSponsorResponse
    {
        public int StayId { get; set; }
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public string Status { get; set; } = null!;
        public RoomShortSponsorResponse Room { get; set; } = null!;
    }
}
