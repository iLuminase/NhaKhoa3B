using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;
using Microsoft.AspNetCore.Authorization;
using MyMvcApp.Services.Interfaces;
using MyMvcApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using MyMvcApp.Data;

namespace MyMvcApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;
    private readonly IDashboardService _dashboardService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public HomeController(
        ILogger<HomeController> logger,
        IUserService userService,
        IDashboardService dashboardService,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context)
    {
        _logger = logger;
        _userService = userService;
        _dashboardService = dashboardService;
        _userManager = userManager;
        _context = context;
    }

    // Action Landing mặc định - luôn hiển thị trang Landing
    public IActionResult Landing()
    {
        return View();
    }

    public async Task<IActionResult> Index()
    {
        // Nếu người dùng chưa đăng nhập, hiển thị trang landing
        if (!User.Identity.IsAuthenticated)
        {
            return View("Landing");
        }

        var currentUser = await _userService.GetCurrentUserAsync(User);
        if (currentUser == null)
        {
            return RedirectToAction("Login", "Account");
        }

        if (await _userService.IsUserInRoleAsync(currentUser, "Admin"))
        {
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
        else if (await _userService.IsUserInRoleAsync(currentUser, "Staff"))
        {
            return RedirectToAction("Index", "Staff");
        }
        else if (await _userService.IsUserInRoleAsync(currentUser, "User"))
        {
            return await UserDashboard();
        }
        else
        {
            return RedirectToAction("Login", "Account");
        }
    }
    private async Task<IActionResult> UserDashboard()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Get user's appointments
        var userAppointments = _context.Appointments
            .Where(a => a.Patient.Email == user.Email)
            .OrderByDescending(a => a.AppointmentDate)
            .Take(5)
            .ToList();

        // Get user's payments/invoices
        var userPayments = _context.Payments
            .Where(p => p.Appointment.Patient.Email == user.Email)
            .OrderByDescending(p => p.PaymentDate)
            .Take(5)
            .ToList();

        ViewBag.UserName = user.FullName;
        ViewBag.RecentAppointments = userAppointments;
        ViewBag.RecentPayments = userPayments;
        ViewBag.TotalAppointments = _context.Appointments.Count(a => a.Patient.Email == user.Email);
        ViewBag.TotalPayments = _context.Payments.Where(p => p.Appointment.Patient.Email == user.Email).Sum(p => p.Amount);

        return View("UserIndex");
    }

    [Authorize(Roles = "User")]
    [HttpGet]
    public async Task<IActionResult> MyProfile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        return View(user);
    }

    [Authorize(Roles = "User")]
    [HttpGet]
    public async Task<IActionResult> UpdateProfile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var model = new UserProfileUpdateViewModel
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            Gender = user.Gender,
            Address = user.Address
        };

        return View(model);
    }

    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<IActionResult> UpdateProfile(UserProfileUpdateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Cập nhật thông tin
        user.FullName = model.FullName;
        user.Email = model.Email;
        user.PhoneNumber = model.PhoneNumber;
        user.DateOfBirth = model.DateOfBirth;
        user.Gender = model.Gender;
        user.Address = model.Address;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Thông tin cá nhân đã được cập nhật thành công!";
            return RedirectToAction(nameof(MyProfile));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [Authorize(Roles = "User")]
    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View(new UserChangePasswordViewModel());
    }

    [Authorize(Roles = "User")]
    [HttpPost]
    public async Task<IActionResult> ChangePassword(UserChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Mật khẩu đã được thay đổi thành công!";
            return RedirectToAction(nameof(MyProfile));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [Authorize(Roles = "User")]
    [HttpGet]
    public async Task<IActionResult> MyAppointments()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var appointments = _context.Appointments
            .Where(a => a.Patient.Email == user.Email)
            .OrderByDescending(a => a.AppointmentDate)
            .ToList();

        return View(appointments);
    }

    [Authorize(Roles = "User")]
    [HttpGet]
    public async Task<IActionResult> MyInvoices()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var payments = _context.Payments
            .Where(p => p.Appointment.Patient.Email == user.Email)
            .OrderByDescending(p => p.PaymentDate)
            .ToList();

        return View(payments);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
