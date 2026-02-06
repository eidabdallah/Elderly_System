using ElderlySystem.DAL.Model;

namespace Elderly_System.DAL.Repositories.Interfaces
{
    public interface IVistorRepository
    {
        Task<int?> GetElderlyIdForSponsorAsync(string sponsorId);

        Task<Visitor?> GetVisitorByPhoneAsync(string phone);
        Task AddVisitorAsync(Visitor visitor);

        Task<bool> ExistsElderlyVisitorAsync(int elderlyId, int visitorId, DateTime date, TimeSpan start, TimeSpan end);

        Task AddElderlyVisitorAsync(ElderlyVisitor link);
    }
}
