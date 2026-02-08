using Elderly_System.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Elderly_System.DAL.DTO.Request.Nurse
{
    public class CompleteNurseProfileRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EducationLevel EducationLevel { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MaritalStatus MaritalStatus { get; set; }
        public string FieldOfStudy { get; set; } = null!;
        public float YearsOfStudy { get; set; }
        public string YearOfGraduation { get; set; } = null!;
        public List<WorkExperienceRequest> WorkExperiences { get; set; } = new List<WorkExperienceRequest>();

    }

}
