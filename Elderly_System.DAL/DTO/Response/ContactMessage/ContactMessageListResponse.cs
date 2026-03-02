using Elderly_System.DAL.Enums;

namespace Elderly_System.DAL.DTO.Response.ContactMessage
{
    public class ContactMessageListResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;

        public string CreatedAt { get; set; } = string.Empty;

        public DateOnly? RepliedAt { get; set; }

        public string RepliedAtDisplay { get; set; } = "لم يتم الرد";

        public string Message { get; set; } = string.Empty;
        public string MessagePreview { get; set; } = string.Empty;

        public string? AdminReply { get; set; }
        public string AdminReplyDisplay { get; set; } = "لم يتم الرد";

        public string Status { get; set; } = null!;
    }

}
