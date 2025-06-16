using Microsoft.AspNetCore.Identity;
using MyMvcApp.Models;

namespace MyMvcApp.Middleware
{
    public class AdminAccessMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AdminAccessMiddleware> _logger;

        public AdminAccessMiddleware(RequestDelegate next, ILogger<AdminAccessMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager)
        {
            var path = context.Request.Path.Value?.ToLower();
            
            // Kiểm tra nếu đang truy cập vào các trang admin
            if (IsAdminPath(path))
            {
                if (!context.User.Identity.IsAuthenticated)
                {
                    _logger.LogWarning("Unauthenticated user attempted to access admin path: {Path}", path);
                    context.Response.Redirect("/Account/Login?returnUrl=" + Uri.EscapeDataString(context.Request.Path));
                    return;
                }

                // Kiểm tra role admin, staff, hoặc dentist
                var user = await userManager.GetUserAsync(context.User);
                if (user == null ||
                    (!await userManager.IsInRoleAsync(user, "Admin") &&
                     !await userManager.IsInRoleAsync(user, "Staff") &&
                     !await userManager.IsInRoleAsync(user, "Dentist")))
                {
                    _logger.LogWarning("User {UserId} attempted to access admin path without permission: {Path}",
                        user?.Id ?? "Unknown", path);
                    context.Response.Redirect("/Account/AccessDenied");
                    return;
                }
            }

            await _next(context);
        }

        private static bool IsAdminPath(string? path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            var adminPaths = new[]
            {
                "/admin", // Areas/Admin
                "/usermanagement",
                "/testdata",
                "/service", // ServiceController có authorize Admin,Staff
                "/appointment", // AppointmentController có authorize Admin,Staff
                "/patient",
                "/report",
                "/payment", // PaymentController nếu có admin functions
                "/dentalrecord" // DentalRecordController nếu có
            };

            // Kiểm tra các đường dẫn cụ thể của admin
            var specificAdminPaths = new[]
            {
                "/account/register", // Chỉ admin mới được tạo user
                "/account/profile", // Profile của admin (khác với user profile)
                "/account/usermanagement"
            };

            return adminPaths.Any(adminPath => path.StartsWith(adminPath, StringComparison.OrdinalIgnoreCase)) ||
                   specificAdminPaths.Any(adminPath => path.Equals(adminPath, StringComparison.OrdinalIgnoreCase));
        }
    }

    public static class AdminAccessMiddlewareExtensions
    {
        public static IApplicationBuilder UseAdminAccess(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AdminAccessMiddleware>();
        }
    }
}
