using Elderly_System.DAL.DTO.Response.ContactMessage;
using Elderly_System.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IContactMessageRepository
    {
        Task<List<ContactMessageListResponse>> GetAdminMessagesAsync();
        Task<ContactMessage?> GetByIdAsync(int id);
        Task SaveChangesAsync();
        Task AddAsync(ContactMessage entity);
    }
}
