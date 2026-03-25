using ElderlySystem.DAL.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elderly_System.DAL.Model
{
    public class Doctor : ApplicationUser
    {
        public string MedicalRank { get; set; } = null!;
        public string YearsOfExperience { get; set; } = null!;
        public int NumberOfOperations { get; set; }
        public DateTime BDate { get; set; }
        public ICollection<DoctorSpecialization> Specializations { get; set; } = new List<DoctorSpecialization>();
        public ICollection<DoctorDisease> Diseases { get; set; } = new List<DoctorDisease>();

        public ICollection<DoctorWorkPlace> WorkPlaces { get; set; } = new List<DoctorWorkPlace>();

        public ICollection<DoctorOperationType> OperationTypes { get; set; } = new List<DoctorOperationType>();

        public ICollection<DoctorMedicalProcedure> MedicalProcedures { get; set; } = new List<DoctorMedicalProcedure>();

        public ICollection<DoctorDiagnosticTest> DiagnosticTests { get; set; } = new List<DoctorDiagnosticTest>();
        public ICollection<DoctorUniversity> Universities { get; set; } = new List<DoctorUniversity>();

        public ICollection<DoctorPreviousWorkPlace> PreviousWorkPlaces { get; set; } = new List<DoctorPreviousWorkPlace>();

        public ICollection<MedicalReport> MedicalReports { get; set; } = new List<MedicalReport>();

        [NotMapped]
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                int age = today.Year - BDate.Year;
                if (BDate.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
    }
}
