using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Middleware;
using MyMvcApp.Data;
using MyMvcApp.Services.Interfaces;
using MyMvcApp.Services.Implementations;
using MyMvcApp.Services;
using MyMvcApp.Areas.Admin.Models;

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
    options.LoginPath = "/Admin/Account/Login";
    options.AccessDeniedPath = "/Admin/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Cookie hết hạn sau 30 phút
    options.SlidingExpiration = true; // Đặt lại thời gian hết hạn khi hoạt động
    options.Cookie.HttpOnly = true; // Ngăn chặn truy cập cookie bằng JavaScript
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Chỉ gửi cookie qua HTTPS
    options.Cookie.SameSite = SameSiteMode.Strict; // Prevent CSRF
});

// Add memory cache
builder.Services.AddMemoryCache();

// Add session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
