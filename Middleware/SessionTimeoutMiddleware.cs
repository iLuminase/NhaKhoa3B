using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MyMvcApp.Middleware
{
    public class SessionTimeoutMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SessionTimeoutMiddleware> _logger;

        public SessionTimeoutMiddleware(RequestDelegate next, ILogger<SessionTimeoutMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                // Check if session exists
                if (!context.Session.IsAvailable)
                {
                    _logger.LogInformation("Session expired for user {UserId}. Redirecting to login page.",
                        context.User.FindFirst("sub")?.Value ?? "unknown");

                    // Session expired, sign out user
                    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    // Redirect to login page
                    context.Response.Redirect("/Identity/Account/Login");
                    return;
                }
            }

            await _next(context);
        }
    }
}