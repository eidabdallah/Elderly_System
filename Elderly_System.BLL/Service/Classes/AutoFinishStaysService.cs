using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Repositories.Interfaces;

namespace Elderly_System.BLL.Service.Classes
{
    public class AutoFinishStaysService : IAutoFinishStaysService
    {
        private readonly IStaySchedulerRepository _repo;

        public AutoFinishStaysService(IStaySchedulerRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> RunAsync(DateTime today)
        {
            var stays = await _repo.GetStaysToAutoFinishAsync(today);
            if (stays.Count == 0) return 0;

            foreach (var stay in stays)
            {
                stay.Status = Status.Finish;

                var room = stay.Room;
                if (room != null)
                {
                    room.CurrentCapacity = Math.Max(0, room.CurrentCapacity
                        - 1);

                    room.Status = (room.CurrentCapacity >= room.Capacity)
                        ? Status.Full
                        : Status.Active;
                }
            }

            await _repo.SaveChangesAsync();
            return stays.Count;
        }
    }
}
