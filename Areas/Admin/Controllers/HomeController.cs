using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Areas.Admin.Models;
using MyMvcApp.Services.Interfaces;

namespace MyMvcApp.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;
    private readonly IDashboardService _dashboardService;

    public HomeController(
        ILogger<HomeController> logger,
        IUserService userService,
        IDashboardService dashboardService)
    {
        _logger = logger;
        _userService = userService;
        _dashboardService = dashboardService;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var currentUser = await _userService.GetCurrentUserAsync(User);
        if (currentUser == null)
        {
            return RedirectToAction("Login", "Account");
        }

        if (await _userService.IsUserInRoleAsync(currentUser, "Admin"))
        {
            try
            {
                var dashboardData = await _dashboardService.GetDashboardDataAsync();
                
                ViewBag.TotalPatients = dashboardData.TotalPatients;
                ViewBag.TodayAppointments = dashboardData.TodayAppointments;
                ViewBag.TotalServices = dashboardData.TotalServices;
                ViewBag.TotalUsers = dashboardData.TotalUsers;
                ViewBag.RecentActivities = new List<object>(); // Temporarily disable to fix type issue
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading dashboard data");
                // Set default values if there's an error
                ViewBag.TotalPatients = 0;
                ViewBag.TodayAppointments = 0;
                ViewBag.TotalServices = 0;
                ViewBag.TotalUsers = 0;
                ViewBag.RecentActivities = new List<object>();
            }

            return View();
        }
        else if (await _userService.IsUserInRoleAsync(currentUser, "Staff"))
        {
            return RedirectToAction("Index", "Staff");
        }
        else
        {
            return RedirectToAction("Index", "Public");
        }
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
