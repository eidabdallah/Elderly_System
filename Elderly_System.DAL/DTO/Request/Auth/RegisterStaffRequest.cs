using Elderly_System.DAL.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Elderly_System.DAL.DTO.Request.Auth
{
    public class RegisterStaffRequest
    {
        [Required] public string FullName { get; set; } = null!;
        [Required, EmailAddress] public string Email { get; set; } = null!;
        [Required(ErrorMessage = "رقم الهاتف مطلوب.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "رقم الهاتف يجب أن يتكون من 10 أرقام.")] 
        public string PhoneNumber { get; set; } = null!;
        [Required] public string City { get; set; } = null!;
        [Required(ErrorMessage = "رقم الهوية مطلوب.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "رقم الهوية يجب أن يتكون من 9 أرقام.")]
        public string NationalId { get; set; } = null!;
        [Required] public Gender Gender { get; set; }
        [Required] public DateTime BirthDate { get; set; }
        [Required] public string Password { get; set; } = null!;

        // Employee
        [Required] public string JobTitle { get; set; } = null!;
        [Required] public DateTime HireDate { get; set; }
        [Required] public EducationLevel EducationLevel { get; set; }
        [Required] public MaritalStatus MaritalStatus { get; set; }

        public string? FieldOfStudy { get; set; }
        public float? YearsOfStudy { get; set; }
        public string? AcademicDegree { get; set; }
        public string? YearDfGraduation { get; set; }

        // If provided => Nurse, else Employee
        public IFormFile? Certificate { get; set; }
    }
}
