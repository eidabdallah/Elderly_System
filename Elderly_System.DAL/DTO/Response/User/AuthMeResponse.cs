using Elderly_System.DAL.Enums;
using System.Text.Json.Serialization;

namespace ElderlySystem.DAL.DTO.Response.User
{
    public class AuthMeResponse
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string city { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        [JsonIgnore]
        public Gender Gender { get; set; }

        public string GenderValue =>
        Gender == Gender.Male ? "ذكر" : "أنثى";
    }
}
