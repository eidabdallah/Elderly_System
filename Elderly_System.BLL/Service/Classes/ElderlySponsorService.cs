using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.DAL.Model;
using Microsoft.AspNetCore.Identity;

namespace Elderly_System.BLL.Service.Classes
{
    public class ElderlySponsorService : IElderlySponsorService
    {
        private readonly IElderlySponsorRepository _repository;
        private readonly IFileService _file;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthenticationService _service;

        public ElderlySponsorService(IElderlySponsorRepository repository, IFileService file, UserManager<ApplicationUser> userManager, IAuthenticationService service)
        {
            _repository = repository;
            _file = file;
            _userManager = userManager;
            _service = service;
        }
    }
}
