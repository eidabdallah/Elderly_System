using Elderly_System.BLL.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elderly_System.PL.Area.Nurse
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Nurse")]
    public class NurseController : ControllerBase
    {
        private readonly IUserService _service;

        public NurseController(IUserService service)
        {
            _service = service;
        }
       
    }
}
