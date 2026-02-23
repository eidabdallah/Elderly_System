using Elderly_System.BLL.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elderly_System.PL.Area.Sponsor.Controller
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Sponsor")]
    [Authorize(Roles = "Sponsor")]
    public class ElderlyController : ControllerBase
    {
        private readonly IElderlySponsorService _service;

        public ElderlyController(IElderlySponsorService service)
        {
            _service = service;
        }
       
    }
}
