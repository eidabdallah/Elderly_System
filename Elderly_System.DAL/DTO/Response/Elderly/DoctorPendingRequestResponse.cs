namespace Elderly_System.DAL.DTO.Response.Elderly
{
    public class DoctorPendingRequestResponse
    {
        public int RequestId { get; set; }
        public int ElderlyId { get; set; }
        public string ElderlyName { get; set; } = null!;
        public string? ComprehensiveExamination { get; set; }
        public string RequestStatus { get; set; } = null!;
    }
}
