using Elderly_System.BLL.Service.Interface;
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

        public UserService(IUserRepository userRepository , UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _repository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<ServiceResult> GetUsersAsync(Status? status = null, Role? role = null)
        {
            var users = await _repository.GetUsersAsync(status);

            var data = new List<UserResponse>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                var roleName = roles.FirstOrDefault();
                Role roleEnum;
                if (!Enum.TryParse(roleName, true, out roleEnum))
                    roleEnum = Role.Employee;

                if (role is not null && roleEnum != role.Value)
                    continue;
                data.Add(new UserResponse
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email ?? "",
                    PhoneNumber = u.PhoneNumber ?? "",
                    City = u.City ?? "",
                    NationalId = u.NationalId ?? "",
                    StatusUser = UserResponse.ToArabic(u.Status),
                    CreatedAt = u.CreatedAt.ToString("yyyy-MM-dd"),
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

            var msg = $"تم تغيير دور المستخدم إلى {ToArabic(newRole)} بنجاح.";
            return ServiceResult.SuccessMessage(msg);
        }

        private static string ToArabic(Role role) => role switch
        {
            Role.Admin => "أدمن",
            Role.Employee => "موظف",
            Role.Nurse => "ممرض",
            Role.Sponsor => "كفيل",
            _ => "غير معروف"
        };
    }
}
