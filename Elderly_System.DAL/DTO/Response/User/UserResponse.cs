using Elderly_System.DAL.Enums;

namespace Elderly_System.DAL.DTO.Response.User
{
    public class UserResponse
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string RoleUser { get; set; } = "";
        public string StatusUser { get; set; } = "";
        public static string ToArabic(Role role) => role switch
        {
            Role.Admin => "أدمن",
            Role.Nurse => "ممرض",
            Role.Sponsor => "كفيل",
            Role.Accountant => "محاسب",
            Role.Chef => "مسؤولة طبخ",
            Role.Security => "حارس",
            Role.Cleaner => "عاملة نظافة",
            Role.Secretary => "سكرتيرة",
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
