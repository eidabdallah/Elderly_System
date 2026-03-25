namespace Elderly_System.DAL.Model
{
    public class DoctorPreviousWorkPlace
    {
        public int Id { get; set; }
        public string WorkPlace { get; set; } = null!;
        public string DoctorId { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
    }
}
