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
        public async Task<Room?> GetRoomByIdWithImagesAsync(int id)
        {
            return await _context.Rooms
                .Include(r => r.RoomImages)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task AddRoomAsync(Room room)
        {
            await _context.AddAsync(room);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Room>> GetAllRoomAsync()
        {
            return await _context.Rooms
                .OrderBy(r => r.RoomNumber)
                .ToListAsync();
        }

        public async Task<bool> DeleteRoomAsync(Room room)
        {
            _context.RoomImages.RemoveRange(room.RoomImages);
            _context.Rooms.Remove(room);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<Room?> GetRoomByIdAsync(int id)
        {
            return await _context.Rooms.FindAsync(id);
        }
        public async Task UpdateRoomAsync(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> UpdateRoomImagesAsync(Room room, List<(string Url, string PublicId)> newImages)
        {
            _context.RoomImages.RemoveRange(room.RoomImages);
            foreach (var (url, publicId) in newImages)
            {
                room.RoomImages.Add(new RoomImage
                {
                    Url = url,
                    PublicId = publicId,
                    RoomId = room.Id
                });
            }
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
