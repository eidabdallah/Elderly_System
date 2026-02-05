using Elderly_System.DAL.Enums;

namespace Elderly_System.DAL.DTO.Response.User
{
    public class UserResponse
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string City { get; set; } = "";
        public string NationalId { get; set; } = "";
        public string RoleUser { get; set; } = "";
        public string StatusUser { get; set; } = "";
        public string CreatedAt { get; set; }
        public static string ToArabic(Role role) => role switch
        {
            Role.Admin => "أدمن",
            Role.Employee => "موظف",
            Role.Nurse => "ممرض",
            Role.Sponsor => "كفيل",
            _ => "غير معروف"
        };
        public static string ToArabic(Status status) => status switch
        {
            Status.Active => "نشط",
            Status.InActive => "غير نشط",
            Status.Pending => "انتظار القبول",
            _ => "غير معروف"
        };
    }

}
