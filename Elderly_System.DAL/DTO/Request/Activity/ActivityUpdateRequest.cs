namespace Elderly_System.DAL.DTO.Request.Activity
{
    public class ActivityUpdateRequest
    {
        public string? ActivityName { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime? Date { get; set; }
        public string? StartTime { get; set; }
        public int? UpdateParticipantId { get; set; }
        public string? UpdateOrganizationName { get; set; }
    }
}
