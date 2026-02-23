using Elderly_System.BLL.Service.Interface;
using Elderly_System.DAL.DTO.Request.Nurse;
using Elderly_System.DAL.DTO.Response.Elderly;
using Elderly_System.DAL.DTO.Response.User;
using Elderly_System.DAL.Enums;
using Elderly_System.DAL.Repositories.Interfaces;
using ElderlySystem.BLL.Helpers;
using ElderlySystem.DAL.Model;
using Microsoft.AspNetCore.Identity;

namespace Elderly_System.BLL.Service.Classes
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthenticationService _service;

        public UserService(IUserRepository userRepository, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager , IAuthenticationService service)
        {
            _repository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _service = service;
        }
        public async Task<ServiceResult> GetUsersAsync(Status? status = null, Role? role = null, string? name = null)
        {
            var users = await _repository.GetUsersAsync(status, name);

            var data = new List<UserResponse>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                var roleName = roles.FirstOrDefault();

                if (string.IsNullOrWhiteSpace(roleName))
                    continue;

                if (!Enum.TryParse<Role>(roleName, true, out var roleEnum))
                    continue;

                if (roleEnum == Role.Admin)
                    continue;

                if (role is not null && roleEnum != role.Value)
                    continue;

                data.Add(new UserResponse
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    StatusUser = UserResponse.ToArabic(u.Status),
                    RoleUser = UserResponse.ToArabic(roleEnum)
                });
            }

            return ServiceResult.SuccessWithData(data, "تم جلب المستخدمين بنجاح");
        }

        public async Task<ServiceResult> ChangeStatusAsync(string userId, Status newStatus)
        {
            if (newStatus != Status.Pending && newStatus != Status.Active && newStatus != Status.InActive)
                return ServiceResult.Failure("الحالة المسموحة فقط: انتظار القبول / نشط / غير نشط.");

            var user = await _repository.GetUserByIdAsync(userId);
            if (user is null)
                return ServiceResult.Failure("المستخدم غير موجود.");

            if (user.Status == newStatus)
                return ServiceResult.SuccessMessage("حالة المستخدم هي نفسها بالفعل.");

            user.Status = newStatus;

            var updated = await _repository.UpdateUserAsync(user);
            if (!updated)
                return ServiceResult.Failure("حدث خطأ أثناء تحديث حالة المستخدم.");

            var msg = newStatus switch
            {
                Status.Active => "تم تفعيل المستخدم بنجاح.",
                Status.InActive => "تم تعطيل المستخدم بنجاح.",
                Status.Pending => "تم إعادة المستخدم إلى حالة انتظار القبول.",
                _ => "تم تحديث الحالة."
            };

            return ServiceResult.SuccessMessage(msg);
        }
        public async Task<ServiceResult> ChangeRoleAsync(string userId, Role newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return ServiceResult.Failure("المستخدم غير موجود.");

            var newRoleName = newRole.ToString();
            var roleExists = await _roleManager.RoleExistsAsync(newRoleName);
            if (!roleExists)
                return ServiceResult.Failure("الدور ليس متوفر");

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any(r => string.Equals(r, newRoleName, StringComparison.OrdinalIgnoreCase)))
                return ServiceResult.SuccessMessage("المستخدم يمتلك نفس الدور بالفعل.");

            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                    return ServiceResult.Failure("فشل حذف الدور الحالي للمستخدم.");
            }
            var addResult = await _userManager.AddToRoleAsync(user, newRoleName);
            if (!addResult.Succeeded)
                return ServiceResult.Failure("فشل تعيين الدور الجديد للمستخدم.");

            var msg = $"تم تغيير دور المستخدم إلى {UserResponse.ToArabic(newRole)} بنجاح.";
            return ServiceResult.SuccessMessage(msg);
        }
        public async Task<ServiceResult> GetUserDetailsAsync(string userId)
        {
            var baseUser = await _repository.GetBaseAsync(userId);
            if (baseUser is null)
                return ServiceResult.Failure("المستخدم غير موجود.");

            var roles = await _userManager.GetRolesAsync(baseUser);
            var roleName = roles.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(roleName))
                return ServiceResult.Failure("لم يتم تعيين صلاحية (Role) لهذا المستخدم.");

            if (!Enum.TryParse<Role>(roleName, true, out var roleEnum))
                return ServiceResult.Failure("صلاحية المستخدم غير معروفة.");

            var dto = new UserDetailsResponse
            {
                Id = baseUser.Id,
                FullName = baseUser.FullName,
                Email = baseUser.Email ?? "",
                PhoneNumber = baseUser.PhoneNumber ?? "",
                City = baseUser.City,
                NationalId = baseUser.NationalId,
                Gender = UserDetailsResponse.ToArabic(baseUser.Gender),
                Status = UserDetailsResponse.ToArabic(baseUser.Status),
                RoleUser = UserDetailsResponse.ToArabic(roleEnum)
            };

            if (roleEnum == Role.Nurse)
            {
                var nurse = await _repository.GetNurseAsync(userId);
                if (nurse is null) return ServiceResult.Failure("بيانات الممرض غير موجودة.");

                FillEmployee(dto, nurse);
                dto.ImageCertificate = nurse.ImageCertificate;
            }
            else if (roleEnum is Role.Accountant or Role.Chef or Role.Secretary or Role.Security or Role.Cleaner)
            {
                var emp = await _repository.GetEmployeeAsync(userId);
                if (emp is null) return ServiceResult.Failure("بيانات الموظف غير موجودة.");

                FillEmployee(dto, emp);
            }
            else if (roleEnum == Role.Sponsor)
            {
                var sponsor = await _repository.GetSponsorWithElderlyAsync(userId);
                if (sponsor is null) return ServiceResult.Failure("بيانات الكفيل غير موجودة.");

                dto.ElderlyList = sponsor.ElderlySponsors
                    .Select(es => new ElderlyMiniDto
                        {
                            Id = es.ElderlyId,
                            Name = es.Elderly.Name
                        })
                        .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                        .GroupBy(x => x.Id)  
                        .Select(g => g.First())
                        .ToList();

            }

            return ServiceResult.SuccessWithData(dto, "تم جلب تفاصيل المستخدم بنجاح");
        }

        private static void FillEmployee(UserDetailsResponse dto, Employee emp)
        {

            dto.EducationLevel = emp.EducationLevel.HasValue
                ? UserDetailsResponse.ToArabic(emp.EducationLevel.Value)
                : null;

            dto.MaritalStatus = emp.MaritalStatus.HasValue
                ? UserDetailsResponse.ToArabic(emp.MaritalStatus.Value)
                : null;

            dto.FieldOfStudy = emp.FieldOfStudy;
            dto.YearsOfStudy = emp.YearsOfStudy;
            dto.YearOfGraduation = emp.YearOfGraduation;

            dto.WorkExperiences = emp.WorkExperiences?
                .Select(w => new WorkExperienceResponse
                {
                    WorkName = w.WorkName ?? "",
                    WorkLocation = w.WorkLocation ?? "",
                })
                .ToList();
        }
        public async Task<ServiceResult> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return ServiceResult.Failure("المستخدم غير موجود.");

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any(r => string.Equals(r, Role.Admin.ToString(), StringComparison.OrdinalIgnoreCase)))
                return ServiceResult.Failure("لا يمكن حذف حساب الأدمن.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var err = string.Join(" | ", result.Errors.Select(e => e.Description));
                return ServiceResult.Failure($"فشل حذف المستخدم. {err}");
            }

            return ServiceResult.SuccessMessage("تم حذف المستخدم بنجاح.");
        }


    }
}
