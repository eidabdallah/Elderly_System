using Elderly_System.DAL.DTO.Response.ContactMessage;
using Elderly_System.DAL.DTO.Response.User;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Model;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using Microsoft.EntityFrameworkCore;
namespace Elderly_System.DAL.Repositories.Classes
{
    public class ContactMessageRepository : IContactMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactMessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<ContactMessageListResponse>> GetAdminMessagesAsync()
        {
            var rows = await _context.ContactMessages
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Select(x => new
                {
                    x.Id,
                    x.FullName,
                    x.Email,
                    x.Subject,
                    x.CreatedAt,
                    x.RepliedAt,
                    x.Message,
                    x.AdminReply,
                    x.Status
                })
                .ToListAsync();

            return rows.Select(x => new ContactMessageListResponse
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Subject = x.Subject,

                CreatedAt = x.CreatedAt.ToString("yyyy-MM-dd"),

                RepliedAt = x.RepliedAt,
                RepliedAtDisplay = x.RepliedAt == null
                    ? "لم يتم الرد"
                    : x.RepliedAt.Value.ToString("yyyy-MM-dd"),

                Message = x.Message,
                MessagePreview = x.Message.Length > 60 ? x.Message[..60] + "..." : x.Message,

                AdminReply = x.AdminReply,
                AdminReplyDisplay = string.IsNullOrWhiteSpace(x.AdminReply) ? "لم يتم الرد" : x.AdminReply,

                Status = x.Status == Status.Finish ? "تم الرد" : "جديد"
            }).ToList();
        }
        public async Task<ContactMessage?> GetByIdAsync(int id)
        {
            return await _context.ContactMessages
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task AddAsync(ContactMessage entity)
        {
            await _context.ContactMessages.AddAsync(entity);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
