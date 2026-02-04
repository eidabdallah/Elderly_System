using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Data;
using ElderlySystem.DAL.Model;
using Microsoft.EntityFrameworkCore;

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
        public async Task<Donation?> GetDonationByIdAsync(int id)
        {
            return await _context.Donations
                .Include(d => d.Goods)
                .FirstOrDefaultAsync(d => d.Id == id);
        }
        public async Task DeleteDonationAsync(Donation donation)
        {
            if (donation.Goods != null && donation.Goods.Count > 0)
                _context.Goods.RemoveRange(donation.Goods);
            _context.Donations.Remove(donation);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateDonationAsync(Donation donation)
        {
            _context.Donations.Update(donation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGoodAsync(Good good)
        {
            _context.Goods.Update(good);
            await _context.SaveChangesAsync();
        }

    }
}
