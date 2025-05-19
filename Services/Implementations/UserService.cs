using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MyMvcApp.Models;
using MyMvcApp.Services.Interfaces;
using MyMvcApp.ViewModels.Admin;
using System.Security.Claims;

namespace MyMvcApp.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMemoryCache _cache;
        private const string USER_CACHE_KEY = "CurrentUserData";

        public UserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMemoryCache cache)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _cache = cache;
        }

        public async Task<IEnumerable<UserManagementViewModel>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var viewModels = new List<UserManagementViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                viewModels.Add(new UserManagementViewModel
                {
                    Id = user.Id ?? string.Empty,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    FullName = user.FullName ?? string.Empty,
                    RoleNames = roles.ToList(),
                    CreatedAt = user.CreatedAt,
                    IsActive = user.IsActive
                });
            }

            return viewModels;
        }

        public async Task<UserEditViewModel?> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            return new UserEditViewModel
            {
                Id = user.Id ?? string.Empty,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName ?? string.Empty,
                CurrentRole = roles.FirstOrDefault() ?? string.Empty,
                DateOfBirth = user.DateOfBirth != default ? user.DateOfBirth.ToDateTime(TimeOnly.MinValue) : null,
                Gender = user.Gender ?? string.Empty,
                IsActive = user.IsActive
            };
        }

        public async Task<ServiceResult> CreateUserAsync(UserCreateViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                DateOfBirth = model.DateOfBirth.HasValue ? DateOnly.FromDateTime(model.DateOfBirth.Value) : default,
                Gender = model.Gender,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.Role))
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                }
                return ServiceResult.SuccessResult();
            }

            return ServiceResult.FailureResult(result.Errors.Select(e => e.Description));
        }

        public async Task<ServiceResult> UpdateUserAsync(UserEditViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return ServiceResult.FailureResult(new[] { "Không tìm thấy người dùng" });

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.FullName = model.FullName;
            user.DateOfBirth = model.DateOfBirth.HasValue ? DateOnly.FromDateTime(model.DateOfBirth.Value) : default;
            user.Gender = model.Gender;
            user.IsActive = model.IsActive;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (currentRoles.Any())
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                }
                if (!string.IsNullOrEmpty(model.CurrentRole))
                {
                    await _userManager.AddToRoleAsync(user, model.CurrentRole);
                }
                return ServiceResult.SuccessResult();
            }

            return ServiceResult.FailureResult(result.Errors.Select(e => e.Description));
        }

        public async Task<ServiceResult> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return ServiceResult.FailureResult(new[] { "Không tìm thấy người dùng" });

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return ServiceResult.SuccessResult();

            return ServiceResult.FailureResult(result.Errors.Select(e => e.Description));
        }

        public async Task<ServiceResult> ToggleUserStatusAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return ServiceResult.FailureResult(new[] { "Không tìm thấy người dùng" });

            user.IsActive = !user.IsActive;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return ServiceResult.SuccessResult();

            return ServiceResult.FailureResult(result.Errors.Select(e => e.Description));
        }

        public async Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
                throw new InvalidOperationException("User not found");
            return user;
        }

        public async Task<bool> IsUserInRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

        public void CacheUserData(ApplicationUser user)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));

            var roles = _userManager.GetRolesAsync(user).Result;
            var userCacheData = new UserCacheData
            {
                Id = user.Id ?? string.Empty,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName ?? string.Empty,
                RoleNames = roles.ToList(),
                IsActive = user.IsActive
            };

            _cache.Set(USER_CACHE_KEY, userCacheData, cacheEntryOptions);
        }

        public UserCacheData GetCachedUserData()
        {
            var cachedData = _cache.Get<UserCacheData>(USER_CACHE_KEY);
            if (cachedData == null)
                throw new InvalidOperationException("No cached user data found");
            return cachedData;
        }
    }
}