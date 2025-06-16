using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace MyMvcApp.Middleware
{
    public class SessionTimeoutMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionTimeoutMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Xử lý session timeout - chỉ áp dụng cho session thường, không ảnh hưởng đến Remember Me
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                // Kiểm tra xem có phải là persistent cookie (Remember Me) không
                var authResult = await context.AuthenticateAsync(IdentityConstants.ApplicationScheme);

                if (authResult.Succeeded && authResult.Properties != null)
                {
                    // Nếu là persistent cookie (Remember Me), bỏ qua kiểm tra session
                    if (authResult.Properties.IsPersistent)
                    {
                        await _next(context);
                        return;
                    }
                }

                // Chỉ kiểm tra session cho đăng nhập thường (không Remember Me)
                if (context.Session.IsAvailable)
                {
                    // Cập nhật thời gian hoạt động cuối
                    context.Session.SetString("LastActivity", DateTime.UtcNow.ToString());
                }
                else
                {
                    // Session không khả dụng và không phải persistent cookie
                    // Chỉ sign out nếu không phải Remember Me
                    if (authResult.Properties?.IsPersistent != true)
                    {
                        await context.SignOutAsync();
                        context.Response.Redirect("/Account/Login?expired=true");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}