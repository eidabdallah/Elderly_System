using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Elderly;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails([FromRoute] int id)
        {
            var result = await _service.GetElderlyDetailsAsync(id);
            return Ok(new { message = result.Message, Elderly = result.Data });
        }
        [HttpGet("medical-reports/{reportId}")]
        public async Task<IActionResult> GetDiagnosis(int reportId)
        {
            var result = await _service.GetMedicalReportDiagnosisAsync(reportId);
            return Ok(new { message = result.Message, Elderly = result.Data });
        }
        [HttpPost("{id}/comprehensive-examination")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadComprehensiveExam(int id, [FromForm] UploadComprehensiveExamRequest request)
        {
            var result = await _service.UploadComprehensiveExamAsync(id, request);
            return Ok(new { message = result.Message});
        }
        [HttpDelete("{id}/comprehensive-examination")]
        public async Task<IActionResult> DeleteComprehensiveExam([FromRoute] int id)
        {
            var result = await _service.DeleteComprehensiveExamAsync(id);
            return Ok(new { message = result.Message });
        }
        [HttpPost("{id}/diseases")]
        public async Task<IActionResult> AddDisease(int id, [FromBody] AddDiseasesRequest request)
        {
            var result = await _service.AddDiseasesAsync(id, request);
            return Ok(new { message = result.Message });
        }
        [HttpPost("{id}/diseases/remove")]
        public async Task<IActionResult> RemoveDisease(int id, [FromBody] RemoveDiseaseRequest request)
        {
            var result = await _service.RemoveDiseaseAsync(id, request);
            return Ok(new { message = result.Message });
        }
        [HttpGet("Doctor")]
        public async Task<IActionResult> GetDoctors()
        {
            var result = await _service.GetDoctorsAsync();
            return Ok(new { message = result.Message, doctors = result.Data });
        }
        [HttpPost("{id}/medical-reports")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddMedicalReport(int id, [FromForm] AddMedicalReportRequest request)
        {
            var result = await _service.AddMedicalReportAsync(id, request);
            return Ok(new { message = result.Message, data = result.Data });
        }

    }
}
