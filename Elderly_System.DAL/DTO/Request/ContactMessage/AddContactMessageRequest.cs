namespace Elderly_System.DAL.DTO.Request.ContactMessage
{
    public class AddContactMessageRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
