using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.DAL.Repositories.Classes
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;
        public RoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CheckRoomNumberAsync(int RoomNumber)
        {
            return await _context.Rooms.AnyAsync(e => e.RoomNumber == RoomNumber);
        }
        public async Task AddRoomAsync(Room room)
        {
            await _context.AddAsync(room);
            await _context.SaveChangesAsync();
        }
    }
}
