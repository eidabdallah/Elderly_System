using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Activity;
using Elderly_System.DAL.DTO.Response.Activity;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;
using ElderlySystem.DAL.Model;

namespace Elderly_System.BLL.Service.Classes
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _repository;

        public ActivityService(IActivityRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult> CreateActivityAsync(ActivityCreateRequest request, string AdminId)
        {

            if (request.ActivityOrganizations == null || request.ActivityOrganizations.Count == 0)
                return ServiceResult.Failure("يجب إضافة جهة منظمة أو مشارك واحد على الأقل.");

            var activity = new Activity
            {
                ActivityName = request.ActivityName,
                Description = request.Description,
                Location = request.Location,
                Date = request.Date.Date,
                StartTime = request.StartTime,
                AdminId = AdminId,
                ActivityOrganizations = request.ActivityOrganizations.Select(p => new Participant
                {
                    OrganizationName = p.OrganizationName
                }).ToList()
            };

            await _repository.AddActivityAsync(activity);
            return ServiceResult.SuccessMessage("تم إضافة النشاط بنجاح.");
        }
        public async Task<List<ActivityResponse>> GetAllActivitiesAsync()
        {
            var activities = await _repository.GetAllActivitiesAsync();

            return activities.Select(a => new ActivityResponse
            {
                Id = a.Id,
                ActivityName = a.ActivityName,
                Description = a.Description,
                Location = a.Location,
                Date = a.Date.ToString("dd/MM/yyyy"),
                StartTime = a.StartTime.ToString(@"hh\:mm"),
                ActivityOrganizations = (a.ActivityOrganizations != null && a.ActivityOrganizations.Count > 0)
                    ? a.ActivityOrganizations.Select(p => new ParticipantResponse
                    {
                        Id = p.Id,
                        OrganizationName = p.OrganizationName
                    }).ToList()
                    : null
            }).ToList();
        }

        public async Task<ActivityResponse?> GetActivityByIdAsync(int id)
        {
            var a = await _repository.GetActivityByIdAsync(id);
            if (a == null) return null;

            return new ActivityResponse
            {
                Id = a.Id,
                ActivityName = a.ActivityName,
                Description = a.Description,
                Location = a.Location,
                Date = a.Date.ToString("dd/MM/yyyy"),
                StartTime = a.StartTime.ToString(@"hh\:mm"),
                ActivityOrganizations = (a.ActivityOrganizations != null && a.ActivityOrganizations.Count > 0)
                    ? a.ActivityOrganizations.Select(p => new ParticipantResponse
                    {
                        Id = p.Id,
                        OrganizationName = p.OrganizationName
                    }).ToList()
                    : null
            };
        }
        public async Task<ServiceResult> DeleteActivityAsync(int activityId)
        {
            var activity = await _repository.GetActivityByIdAsync(activityId);
            if (activity == null)
                return ServiceResult.Failure("النشاط غير موجود.");

            await _repository.DeleteActivityAsync(activity);
            return ServiceResult.SuccessMessage("تم حذف النشاط بنجاح.");
        }
    }
}
