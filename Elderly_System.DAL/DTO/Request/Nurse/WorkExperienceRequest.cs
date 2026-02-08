namespace Elderly_System.DAL.DTO.Request.Nurse
{
    public class WorkExperienceRequest
    {
        public string WorkName { get; set; } = null!;
        public string? WorkLocation { get; set; }
        public string JobTitle { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
