namespace Elderly_System.DAL.DTO.Response.Sponsor
{
    public class SponsorElderlyBriefDto
    {
        public int ElderlyId { get; set; }
        public string ElderlyName { get; set; } = "";
        public SponsorRelationDto? Sponsor { get; set; }
    }
}
