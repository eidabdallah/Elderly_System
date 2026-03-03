using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Medicine;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Elderly_System.PL.Area.Nurse
{
    [Route("api/[area]/[controller]")]
    [Area("Nurse")]
    [ApiController]
    [Authorize(Roles = "Nurse")]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService _service;

        public MedicineController(IMedicineService service)
        {
            _service = service;
        }
        [HttpPost("drug-plan")]
        public async Task<IActionResult> AddDrugPlan([FromBody] AddDrugPlanRequest request)
        {
            var result = await _service.AddDrugPlanAsync(request);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int? type)
        {
            var data = await _service.GetAllMedicinesAsync(search, type);
            return Ok(new { message = "تم جلب الأدوية بنجاح", data });
        }
        [HttpGet("elderly/{elderlyId}/drug-plans")]
        public async Task<IActionResult> GetElderlyDrugPlans([FromRoute] int elderlyId)
        {
            var data = await _service.GetElderlyDrugPlansAsync(elderlyId);

            if (data == null)
                return NotFound(new { message = "المسن غير موجود." });

            return Ok(new { message = "تم جلب أدوية المسن بنجاح", data });
        }
        [HttpPut("drug-plan/{id}")]
        public async Task<IActionResult> UpdateDrugPlan([FromRoute] int id, [FromBody] DrugPlanUpdateRequest request)
        {
            var result = await _service.UpdateDrugPlanAsync(id, request);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        [HttpPut("drug-plan/{id}/status")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int id, [FromQuery] int status)
        {
            var result = await _service.UpdateDrugPlanStatusAsync(id, status);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
        [HttpPut("drug-plan/{id}/medicine-status")]
        public async Task<IActionResult> UpdateStatusMed([FromRoute] int id, [FromQuery] int medicineStatus)
        {
            var result = await _service.UpdateDrugPlanStatusMedAsync(id, medicineStatus);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
        [HttpPost("Medication")]
        public async Task<IActionResult> CreateMedication([FromBody] MedicationCreateRequest request)
        {
            var nurseId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(nurseId))
                return Unauthorized(new { message = "تعذر تحديد المستخدم من التوكن." });

            var result = await _service.AddMedicationAsync(request, nurseId);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
        [HttpGet("elderly/{elderlyId}/Medicine")]
        public async Task<IActionResult> GetElderlyMedicine([FromRoute] int elderlyId)
        {
            var data = await _service.GetElderlyMedicineAsync(elderlyId);

            if (data == null)
                return NotFound(new { message = "المسن غير موجود." });

            return Ok(new { message = "تم جلب أدوية المسن بنجاح", data });
        }
        [HttpGet("elderly/{elderlyId}/weekly")]
        public async Task<IActionResult> GetElderlyWeekly([FromRoute] int elderlyId, [FromQuery] int offset = 0)
        {
            var result = await _service.GetElderlyWeeklyMedicationScheduleAsync(elderlyId, offset);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message, data = result.Data });
        }
    }
}
