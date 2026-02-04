using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Classes
{
    public class DonationRepository : IDonationRepository
    {
        private readonly ApplicationDbContext _context;

        public DonationRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddDonationAsync(Donation donation)
        {
            await _context.AddAsync(donation);
            await _context.SaveChangesAsync();
        }

    }
}
