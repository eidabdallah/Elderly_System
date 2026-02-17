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
                StartTime = request.StartTime.Trim(),
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
                Location = a.Location,
                Date = a.Date.ToString("dd/MM/yyyy"),
                StartTime = a.StartTime,
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
                StartTime = a.StartTime,
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
        public async Task<ServiceResult> UpdateActivityAsync(int activityId, ActivityUpdateRequest request)
        {
            var activity = await _repository.GetActivityByIdAsync(activityId);
            if (activity == null)
                return ServiceResult.Failure("النشاط غير موجود.");

            bool sentAny = request.ActivityName != null || request.Description != null || request.Location != null || request.Date.HasValue ||
                request.StartTime != null || request.UpdateParticipantId.HasValue || request.UpdateOrganizationName != null;

            if (!sentAny)
                return ServiceResult.Failure("لم يتم إرسال أي بيانات للتعديل.");

            bool activityChanged = request.ActivityName != null || request.Description != null || request.Location != null ||
                request.Date.HasValue || request.StartTime != null;

            if (request.ActivityName != null)
            {
                if (string.IsNullOrWhiteSpace(request.ActivityName))
                    return ServiceResult.Failure("اسم النشاط لا يمكن أن يكون فارغًا.");
                activity.ActivityName = request.ActivityName.Trim();
            }
            if (request.Description != null)
            {
                activity.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
            }
            if (request.Location != null)
            {
                if (string.IsNullOrWhiteSpace(request.Location))
                    return ServiceResult.Failure("الموقع لا يمكن أن يكون فارغًا.");
                activity.Location = request.Location.Trim();
            }

            if (request.Date.HasValue)
                activity.Date = request.Date.Value.Date;

            if (request.StartTime != null)
                activity.StartTime = request.StartTime;

            if (activityChanged)
                await _repository.UpdateActivityAsync(activity);

            bool participantUpdateSent = request.UpdateParticipantId.HasValue || request.UpdateOrganizationName != null;

            if (participantUpdateSent)
            {
                if (!request.UpdateParticipantId.HasValue)
                    return ServiceResult.Failure("معرف المشارك مطلوب لتعديل الجهة المشاركة.");

                if (request.UpdateOrganizationName == null)
                    return ServiceResult.Failure("اسم المشارك مطلوب لتعديل الجهة المشاركة.");

                if (string.IsNullOrWhiteSpace(request.UpdateOrganizationName))
                    return ServiceResult.Failure("اسم الجهة لا يمكن أن يكون فارغًا.");

                var participant = activity.ActivityOrganizations
                    .FirstOrDefault(p => p.Id == request.UpdateParticipantId.Value);

                if (participant == null)
                    return ServiceResult.Failure("الجهة المشاركة غير موجودة ضمن هذا النشاط.");
                if (participant.ActivityId != activityId)
                    return ServiceResult.Failure("الجهة المشاركة غير تابعة لهذا النشاط.");
                participant.OrganizationName = request.UpdateOrganizationName.Trim();
                await _repository.UpdateParticipantAsync(participant);
            }

            return ServiceResult.SuccessMessage("تم تعديل النشاط بنجاح.");
        }
    }
}
