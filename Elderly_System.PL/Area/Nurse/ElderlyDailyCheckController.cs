using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Elderly;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Elderly_System.PL.Area.Nurse
{
    [Route("api/[area]/[controller]")]
    [Area("Nurse")]
    [ApiController]
    [Authorize(Roles = "Nurse")]

    public class ElderlyDailyCheckController : ControllerBase
    {
        private readonly ICheckListService _service;

        public ElderlyDailyCheckController(ICheckListService service) {
            _service = service;
        }
        [HttpPost("")]
        public async Task<IActionResult> AddCheckList([FromBody] AddCheckListRequest request)
        {
            var nurseId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(nurseId))
                return Unauthorized(new { message = "الممرضة غير معروفة." });

            var result = await _service.AddCheckListAsync(request, nurseId);
            return Ok(new { message = result.Message });
        }

        [HttpGet("elderly/{elderlyId}")]
        public async Task<IActionResult> GetCheckListsByElderlyId([FromRoute] int elderlyId)
        {
            var result = await _service.GetCheckListsByElderlyIdAsync(elderlyId);
            return Ok(new { message = result.Message, data = result.Data });
        }

        [HttpGet("{checkListId}")]
        public async Task<IActionResult> GetCheckListById([FromRoute] int checkListId)
        {
            var result = await _service.GetCheckListByIdAsync(checkListId);
            return Ok(new { message = result.Message, data = result.Data });
        }

        [HttpPut("{checkListId}")]
        public async Task<IActionResult> UpdateCheckList([FromRoute] int checkListId, [FromBody] UpdateCheckListRequest request)
        {
            var result = await _service.UpdateCheckListAsync(checkListId, request);
            return Ok(new { message = result.Message });
        }

        [HttpDelete("{checkListId}")]
        public async Task<IActionResult> DeleteCheckList([FromRoute] int checkListId)
        {
            var result = await _service.DeleteCheckListAsync(checkListId);
            return Ok(new { message = result.Message });
        }
    }
}
