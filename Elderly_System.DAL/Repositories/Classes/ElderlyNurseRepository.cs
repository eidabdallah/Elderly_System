using Elderly_System.DAL.DTO.Response.Nurse;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace Elderly_System.DAL.Repositories.Classes
{
    public class ElderlyNurseRepository : IElderlyNurseRepository
    {
        private readonly ApplicationDbContext _context;

        public ElderlyNurseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<NurseElderlyListDto>> GetActiveResidentElderliesAsync()
        {
            return await _context.Elderlies
                .AsNoTracking()
                .Where(e =>
                    e.status == Status.Active &&
                    e.ResidentStays.Any(rs => rs.Status == Status.Active))
                .Select(e => new NurseElderlyListDto
                {
                    ElderlyId = e.Id,
                    Name = e.Name,
                    RoomNumber = e.ResidentStays
                        .Where(rs => rs.Status == Status.Active)
                        .Select(rs => rs.Room.RoomNumber) 
                        .FirstOrDefault()
                })
                .OrderBy(x => x.RoomNumber)
                .ToListAsync();
        }
    }
}
