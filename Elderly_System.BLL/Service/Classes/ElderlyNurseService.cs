using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;

namespace Elderly_System.BLL.Service.Classes
{
    public class ElderlyNurseService : IElderlyNurseService
    {
        private readonly IElderlyNurseRepository _repository;

        public ElderlyNurseService(IElderlyNurseRepository repository)
        {
            _repository = repository;
        }
        public async Task<ServiceResult> GetActiveResidentElderliesAsync()
        {
            var data = await _repository.GetActiveResidentElderliesAsync();
            return ServiceResult.SuccessWithData(data, "تم جلب المسنين بنجاح");
        }
    }
}
