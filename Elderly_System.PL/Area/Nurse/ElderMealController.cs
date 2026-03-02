using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Elderly;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elderly_System.PL.Area.Nurse
{
    [Route("api/[area]/[controller]")]
    [Area("Nurse")]
    [ApiController]
    [Authorize(Roles = "Nurse")]
    public class ElderMealController : ControllerBase
    {
        private readonly IElderMealService _service;

        public ElderMealController(IElderMealService service)
        {
            _service = service;
        }
        [HttpPost("daily")]
        public async Task<IActionResult> AddDailyMeals([FromBody] AddDailyMealsRequest request)
        {
            var result = await _service.AddDailyMealsWithDetailsAsync(request);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
        [HttpGet("weekly")]
        public async Task<IActionResult> GetWeekly([FromQuery] int offset = 0, [FromQuery] int? elderlyId = null)
        {
            var result = await _service.GetWeeklyMealsAsync(offset, elderlyId);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message, data = result.Data });
        }
    }
}
