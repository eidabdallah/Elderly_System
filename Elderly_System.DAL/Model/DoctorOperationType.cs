namespace Elderly_System.DAL.Model
{
    public class DoctorOperationType
    {
        public int Id { get; set; }
        public string OperationType { get; set; } = null!;
        public string DoctorId { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
    }
}
