namespace Elderly_System.DAL.Model
{
    public class DoctorDisease
    {
        public int Id { get; set; }
        public string Disease { get; set; } = null!;
        public string DoctorId { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
    }
}
