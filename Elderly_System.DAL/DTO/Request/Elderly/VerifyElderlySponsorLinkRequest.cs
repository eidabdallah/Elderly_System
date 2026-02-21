namespace Elderly_System.DAL.DTO.Request.Elderly
{
    public class VerifyElderlySponsorLinkRequest
    {
        public string ElderlyNationalId { get; set; } = string.Empty;
        public string SponsorNationalId { get; set; } = string.Empty;
    }
}
