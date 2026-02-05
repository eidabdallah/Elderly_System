using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.Repositories.Interfaces;

namespace Elderly_System.BLL.Service.Classes
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository userRepository)
        {
            _repository = userRepository;
        }
    }
}
