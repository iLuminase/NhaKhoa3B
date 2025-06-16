using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Middleware;
using MyMvcApp.Data;
using MyMvcApp.Models;
using MyMvcApp.Services.Interfaces;
using MyMvcApp.Services.Implementations;
using MyMvcApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.SignIn.RequireConfirmedAccount = false; // Changed to false for testing
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Cài đặt cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.LogoutPath = "/Account/Logout";

    // Cấu hình thời gian cookie
    options.ExpireTimeSpan = TimeSpan.FromDays(7); // Cookie hết hạn sau 7 ngày (cho Remember Me)
    options.SlidingExpiration = true; // Đặt lại thời gian hết hạn khi hoạt động

    // Cấu hình cookie security
    options.Cookie.HttpOnly = true; // Ngăn chặn truy cập cookie bằng JavaScript
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Hỗ trợ HTTP trong development
    options.Cookie.SameSite = SameSiteMode.Lax; // Tương thích tốt hơn
    options.Cookie.IsEssential = true; // Cookie cần thiết cho ứng dụng
    options.Cookie.Name = "DentalAuth"; // Tên cookie tùy chỉnh

    // Cấu hình persistent cookie cho Remember Me
    options.Events.OnSigningIn = context =>
    {
        // Nếu Remember Me được chọn, cookie sẽ persistent
        if (context.Properties.IsPersistent)
        {
            context.Properties.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30); // 30 ngày cho Remember Me
        }
        else
        {
            context.Properties.ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2); // 2 giờ cho session thường
        }
        return Task.CompletedTask;
    };

    options.ReturnUrlParameter = "returnUrl";
});

// Add memory cache
builder.Services.AddMemoryCache();

// Add session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2); // Tăng thời gian session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.Name = "DentalSession"; // Tên session cookie tùy chỉnh
});

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add Email Sender
// builder.Services.AddScoped<IEmailSender<ApplicationUser>, EmailSender>();
builder.Services.AddScoped<IEmailSender<ApplicationUser>, CustomEmailSender>();

// Register custom services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IMoMoService, MoMoService>();

// Add HttpClient for MoMo API
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession(); // Add this before UseAuthentication
app.UseMiddleware<SessionTimeoutMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

// Custom middleware để kiểm tra quyền truy cập admin
app.UseAdminAccess();

// Configure Areas routing
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Route cho Admin area
app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{action=Index}/{id?}",
    defaults: new { area = "Admin", controller = "Home" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Landing}/{id?}");

// Route cho trang đăng nhập
app.MapControllerRoute(
    name: "login",
    pattern: "login",
    defaults: new { controller = "Account", action = "Login" });

// Route cho trang đăng ký
app.MapControllerRoute(
    name: "register",
    pattern: "register",
    defaults: new { controller = "Account", action = "UserRegister" });

// Route để handle logout từ Admin area (redirect về Account controller)
app.MapControllerRoute(
    name: "admin-logout-redirect",
    pattern: "Admin/Account/Logout",
    defaults: new { controller = "Account", action = "LogoutFromAdmin" });

app.Run();
