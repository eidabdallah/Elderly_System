using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Activity;
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
    }
}
