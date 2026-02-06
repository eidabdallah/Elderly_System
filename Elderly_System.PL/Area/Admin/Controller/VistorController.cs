using Elderly_System.BLL.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elderly_System.PL.Area.Admin.Controller
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class VisitorController : ControllerBase
    {
        private readonly IVistorService _service;

        public VisitorController(IVistorService service)
        {
            _service = service;
        }
    }
}
