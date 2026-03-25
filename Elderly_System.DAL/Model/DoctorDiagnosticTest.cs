namespace Elderly_System.DAL.Model
{
    public class DoctorDiagnosticTest
    {
        public int Id { get; set; }
        public string TestName { get; set; } = null!;
        public string DoctorId { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
    }
}
