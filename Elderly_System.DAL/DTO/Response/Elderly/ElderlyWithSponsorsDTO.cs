namespace Elderly_System.DAL.DTO.Response.Elderly
{
    public class ElderlyWithSponsorsDto
    {
        public int ElderlyId { get; set; }
        public string ElderlyName { get; set; } = null!;
        public string ReasonRegister { get; set; } = null!;
        public List<ElderlySponsorBriefDTO> Sponsors { get; set; } = new();
    }
}
