using Elderly_System.DAL.Enums;

namespace Elderly_System.DAL.Model
{
    public class ContactMessage
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Status Status { get; set; }= Status.Active;
        public string? AdminReply { get; set; }
        public DateOnly? RepliedAt { get; set; }
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    }
}
