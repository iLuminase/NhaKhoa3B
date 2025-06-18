using Microsoft.AspNetCore.Identity;
using MyMvcApp.Areas.Admin.Models;
using MyMvcApp.ViewModels.Admin;
using System.Security.Claims;

namespace MyMvcApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserManagementViewModel>> GetAllUsersAsync();
        Task<UserEditViewModel?> GetUserByIdAsync(string id);
        Task<ServiceResult> CreateUserAsync(UserCreateViewModel model);
        Task<ServiceResult> UpdateUserAsync(UserEditViewModel model);
        Task<ServiceResult> DeleteUserAsync(string id);
        Task<ServiceResult> ToggleUserStatusAsync(string id);
        Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal principal);
        Task<bool> IsUserInRoleAsync(ApplicationUser user, string role);
        void CacheUserData(ApplicationUser user);
        UserCacheData GetCachedUserData();
    }

    public class ServiceResult
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();

        public static ServiceResult SuccessResult()
        {
            return new ServiceResult { Success = true };
        }

        public static ServiceResult FailureResult(IEnumerable<string> errors)
        {
            return new ServiceResult { Success = false, Errors = errors };
        }
    }
} 