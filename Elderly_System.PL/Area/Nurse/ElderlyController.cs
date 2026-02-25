using Elderly_System.BLL.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elderly_System.PL.Area.Nurse
{
    [Route("api/[area]/[controller]")]
    [Area("Nurse")]
    [ApiController]
    [Authorize(Roles = "Nurse")]
    public class ElderlyController : ControllerBase
    {
        private readonly IElderlyNurseService _service;

        public  ElderlyController(IElderlyNurseService service)
        {
            _service = service;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetActiveElderlies()
        {
            var result = await _service.GetActiveResidentElderliesAsync();
            return Ok(new { message = result.Message, Elderlies = result.Data });
        }
    }
}
