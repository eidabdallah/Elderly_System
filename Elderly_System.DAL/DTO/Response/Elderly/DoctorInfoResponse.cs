namespace Elderly_System.DAL.DTO.Response.Elderly
{
    public class DoctorInfoResponse
    {
        public int DoctorId { get; set; }
        public string Name { get; set; } = null!;
        public string WorkPlace { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
}
