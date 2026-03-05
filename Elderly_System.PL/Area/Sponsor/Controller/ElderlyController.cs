using Elderly_System.BLL.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Elderly_System.PL.Area.Sponsor.Controller
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Sponsor")]
    [Authorize(Roles = "FirstSponsor,SecondSponsor")]
    public class ElderlyController : ControllerBase
    {
        private readonly IElderlySponsorService _service;

        public ElderlyController(IElderlySponsorService service)
        {
            _service = service;
        }
        [HttpGet("mine")]
        public async Task<IActionResult> GetMyElderlies()
        {
            var sponsorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.GetMyElderliesAsync(sponsorId!);
            return Ok(result);
        }
        [HttpGet("{elderlyId}/details")]
        public async Task<IActionResult> GetDetails(int elderlyId)
        {
            var sponsorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.GetElderlyDetailsForSponsorAsync(sponsorId!, elderlyId);
            return Ok(result);
        }
        [HttpGet("medical-reports/{reportId}")]
        public async Task<IActionResult> GetMedicalReportDiagnosis(int reportId)
        {
            var result = await _service.GetMedicalReportDiagnosisAsync(reportId);
            return Ok(new { message = result.Message, data = result.Data });
        }
        [HttpGet("mine/medicines")]
        public async Task<IActionResult> GetMyElderliesMedicines()
        {
            var sponsorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.GetMyElderliesMedicinesAsync(sponsorId!);
            return Ok(result);
        }
        [HttpGet("mine/checklists/today")]
        public async Task<IActionResult> GetTodayChecklists()
        {
            var sponsorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _service.GetMyElderliesTodayChecklistsAsync(sponsorId!);
            return Ok(result);
        }

    }
}
