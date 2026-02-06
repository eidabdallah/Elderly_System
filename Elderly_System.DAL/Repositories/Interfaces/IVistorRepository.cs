using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IVistorRepository
    {
        Task<int?> GetElderlyIdForSponsorAsync(string sponsorId);

        Task<Visitor?> GetVisitorByPhoneAsync(string phone);
        Task AddVisitorAsync(Visitor visitor);

        Task AddElderlyVisitorAsync(ElderlyVisitor link);

        // admin flow
        Task<List<ElderlyVisitor>> GetPendingRequestsAsync();
        Task<ElderlyVisitor?> GetRequestByIdAsync(int requestId);
        Task UpdateAsync(ElderlyVisitor req);

        Task<List<string>> GetSponsorEmailsByElderlyIdAsync(int elderlyId);
    }
}
