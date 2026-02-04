using System.Text.Json.Serialization;

namespace Elderly_System.DAL.DTO.Response.Activity
{
    public class ActivityResponse
    {
        public int Id { get; set; }
        public string ActivityName { get; set; } = null!;
        public string? Description { get; set; }
        public string Location { get; set; } = null!;
        public string Date { get; set; } = null!;
        public string StartTime { get; set; } = null!;
        public List<ParticipantResponse>? ActivityOrganizations { get; set; }
    }
}
