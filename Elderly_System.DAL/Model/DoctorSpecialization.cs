namespace Elderly_System.DAL.Model
{
    public class DoctorSpecialization
    {
        public int Id { get; set; }
        public string Specialization { get; set; } = null!;
        public string DoctorId { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
    }
}
