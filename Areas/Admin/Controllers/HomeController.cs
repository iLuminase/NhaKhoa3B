using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Attributes;
using Microsoft.AspNetCore.Identity;
using MyMvcApp.Models;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;

namespace MyMvcApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeRoles("Admin")]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get dashboard statistics
            ViewBag.TotalPatients = await _context.Patients.CountAsync();
            ViewBag.TodayAppointments = await _context.Appointments
                .CountAsync(a => a.AppointmentDate.Date == DateTime.Today);
            ViewBag.TotalServices = await _context.Services.CountAsync();
            ViewBag.TotalUsers = await _userManager.Users.CountAsync();

            // Get recent activities from Activities table
            var recentActivities = await _context.Activities
                .Include(a => a.User)
                .OrderByDescending(a => a.Time)
                .Take(10)
                .Select(a => new
                {
                    Time = a.Time,
                    Description = a.Description,
                    User = a.User.FullName
                })
                .ToListAsync();

            ViewBag.RecentActivities = recentActivities;

            return View();
        }

        public IActionResult Users()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CreateRoles()
        {
            string[] roleNames = { "Admin", "Staff", "User" };
            foreach (var roleName in roleNames)
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            return Content("Roles created successfully!");
        }

        public async Task<IActionResult> CreateAdmin()
        {
            // Create admin user
            var adminUser = new ApplicationUser
            {
                UserName = "admin@dental.com",
                Email = "admin@dental.com",
                FullName = "System Administrator",
                DateOfBirth = new DateOnly(1990, 1, 1),
                Gender = "Male",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(adminUser, "Admin@123");

            if (result.Succeeded)
            {
                // Assign admin role
                await _userManager.AddToRoleAsync(adminUser, "Admin");
                return Content("Admin account created successfully!");
            }

            return Content("Failed to create admin account: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<IActionResult> CreateStaff()
        {
            // Create staff user
            var staffUser = new ApplicationUser
            {
                UserName = "staff@dental.com",
                Email = "staff@dental.com",
                FullName = "Nhân viên Y tế",
                DateOfBirth = new DateOnly(1992, 5, 15),
                Gender = "Female",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(staffUser, "Staff@123");

            if (result.Succeeded)
            {
                // Assign staff role
                await _userManager.AddToRoleAsync(staffUser, "Staff");
                return Content("Staff account created successfully! Email: staff@dental.com, Password: Staff@123");
            }

            return Content("Failed to create staff account: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<IActionResult> CreateDentist()
        {
            // Create dentist user
            var dentistUser = new ApplicationUser
            {
                UserName = "dentist@dental.com",
                Email = "dentist@dental.com",
                FullName = "Bác sĩ Nha khoa",
                DateOfBirth = new DateOnly(1985, 8, 20),
                Gender = "Male",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(dentistUser, "Dentist@123");

            if (result.Succeeded)
            {
                // Assign dentist role
                await _userManager.AddToRoleAsync(dentistUser, "Dentist");
                return Content("Dentist account created successfully! Email: dentist@dental.com, Password: Dentist@123");
            }

            return Content("Failed to create dentist account: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public IActionResult SidebarDemo()
        {
            return View();
        }
    }
}
