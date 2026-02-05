using Elderly_System.DAL.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elderly_System.DAL.DTO.Request.Elderly
{
    public class AddElderlyWithDoctorRequest
    {
        [Required] public string Name { get; set; } = null!;
        [Required(ErrorMessage = "رقم الهوية مطلوب.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "رقم الهوية يجب أن يتكون من 9 أرقام.")]
        public string NationalId { get; set; } = null!;
        [Required] public string Doctrine { get; set; } = null!;
        [Required] public MaritalStatus MaritalStatus { get; set; }
        [Required] public string City { get; set; } = null!;
        [Required] public string Street { get; set; } = null!;
        [Required] public string HealthStatus { get; set; } = null!;
        [Required] public DateTime BDate { get; set; }
        [Required] public string ReasonRegister { get; set; } = null!;
        public List<string> Diseases { get; set; } = new();
        [Required] public IFormFile NationalIdImage { get; set; } = null!;
        [Required] public IFormFile HealthInsurance { get; set; } = null!;

        [Required] public string KinShip { get; set; } = null!;
        [Required] public string Degree { get; set; } = null!;

        [Required] public string DoctorName { get; set; } = null!;
        [Required] public string WorkPlace { get; set; } = null!;

        [Required(ErrorMessage = "رقم الهاتف مطلوب.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "رقم الهاتف يجب أن يتكون من 10 أرقام.")]
        public string DoctorPhone { get; set; } = null!;

        [Required] public DateTime ReportDate { get; set; }
        [Required] public IFormFile DiagnosisFile { get; set; } = null!;
    }
}
