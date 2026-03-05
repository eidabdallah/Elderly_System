namespace Elderly_System.DAL.DTO.Response.Sponsor
{
    public class SponsorElderlyTodayChecklistsResponse
    {
        public int ElderlyId { get; set; }
        public string ElderlyName { get; set; } = "";
        public List<SponsorChecklistItemResponse> CheckLists { get; set; } = new();
    }
}
