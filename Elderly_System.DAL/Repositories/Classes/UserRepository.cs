using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;

namespace Elderly_System.DAL.Repositories.Classes
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
