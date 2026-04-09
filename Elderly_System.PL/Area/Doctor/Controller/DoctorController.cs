using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Doctor;
using Elderly_System.DAL.DTO.Request.Elderly;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Elderly_System.PL.Area.Doctor.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _service;

        public DoctorController(IDoctorService service)
        {
            _service = service;
        }
        [HttpGet("mine/elderlies")]
        public async Task<IActionResult> GetMyElderlies()
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.GetMyElderliesAsync(doctorId!);
            return Ok(result);
        }
        [HttpPost("medical-reports")]
        public async Task<IActionResult> AddMedicalReport([FromForm] AddMedicalReportDto dto)
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.AddMedicalReportAsync(doctorId!, dto);
            return Ok(result);
        }
        [HttpGet("doctor-change-requests/pending")]
        public async Task<IActionResult> GetPendingDoctorRequests()
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.GetPendingDoctorRequestsAsync(doctorId!);
            return Ok(result);
        }

        [HttpPut("doctor-change-requests/{requestId}/approve")]
        public async Task<IActionResult> ApproveDoctorRequest(int requestId)
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.UpdateDoctorRequestStatusAsync(doctorId!, requestId, true);
            return Ok(result);
        }

        [HttpPut("doctor-change-requests/{requestId}/reject")]
        public async Task<IActionResult> RejectDoctorRequest(int requestId)
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.UpdateDoctorRequestStatusAsync(doctorId!, requestId, false);
            return Ok(result);
        }
        [HttpGet("profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.GetDoctorProfileAsync(doctorId!);
            return Ok(result);
        }

        [HttpPatch("profile")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateDoctorProfileRequest request)
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.UpdateDoctorProfileAsync(doctorId!, request);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails([FromRoute] int id)
        {
            var result = await _service.GetDoctorElderlyDetailsAsync(id);
            return Ok(new { message = result.Message, data = result.Data });
        }

        [HttpGet("medical-reports/{reportId}")]
        public async Task<IActionResult> GetDiagnosis([FromRoute] int reportId)
        {
            var result = await _service.GetDoctorMedicalReportDiagnosisAsync(reportId);
            return Ok(new { message = result.Message, data = result.Data });
        }
    }
}
