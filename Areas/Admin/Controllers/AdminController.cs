using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.ViewModels;
using MyMvcApp.Data;
using MyMvcApp.Areas.Admin.Models;

namespace MyMvcApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeRoles("Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AdminController(
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
                    a.Time,
                    a.Description,
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

        public async Task<IActionResult> Services()
        {
            var services = await _context.Services
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
            return View(services);
        }

        public async Task<IActionResult> Appointments()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Service)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
            return View(appointments);
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

        public async Task<IActionResult> UserManagement()
        {
            var users = await _userManager.Users
                .Select(u => new ApplicationUser
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    CreatedAt = u.CreatedAt,
                    IsActive = u.IsActive
                })
                .ToListAsync();

            var userViewModels = new List<UserManagementViewModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserManagementViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                    IsActive = user.IsActive,
                    RoleNames = roles.ToList()
                });
            }

            return View(userViewModels);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            ViewBag.Roles = _roleManager.Roles.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                    return RedirectToAction(nameof(UserManagement));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewBag.Roles = _roleManager.Roles.ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                IsActive = user.IsActive,
                CurrentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
            };

            ViewBag.Roles = _roleManager.Roles.ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                user.FullName = model.FullName;
                user.DateOfBirth = model.DateOfBirth;
                user.Gender = model.Gender;
                user.IsActive = model.IsActive;

                // Ensure SecurityStamp is set if it's null
                if (string.IsNullOrEmpty(user.SecurityStamp))
                {
                    user.SecurityStamp = Guid.NewGuid().ToString();
                }

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    // Update role if changed
                    var currentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                    if (currentRole != model.CurrentRole)
                    {
                        if (currentRole != null)
                        {
                            await _userManager.RemoveFromRoleAsync(user, currentRole);
                        }
                        await _userManager.AddToRoleAsync(user, model.CurrentRole);
                    }

                    return RedirectToAction(nameof(UserManagement));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewBag.Roles = _roleManager.Roles.ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleUserStatus(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng" });
            }

            user.IsActive = !user.IsActive;

            // Ensure SecurityStamp is set if it's null
            if (string.IsNullOrEmpty(user.SecurityStamp))
            {
                user.SecurityStamp = Guid.NewGuid().ToString();
            }
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Có lỗi xảy ra khi cập nhật trạng thái" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng" });
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Có lỗi xảy ra khi xóa người dùng" });
        }

        public async Task<IActionResult> Patients()
        {
            var patients = await _context.Patients
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return View(patients);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatient(Patient patient)
        {
            if (ModelState.IsValid)
            {
                patient.CreatedAt = DateTime.Now;
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePatient(Patient patient)
        {
            if (ModelState.IsValid)
            {
                var existingPatient = await _context.Patients.FindAsync(patient.Id);
                if (existingPatient == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy bệnh nhân" });
                }

                existingPatient.FullName = patient.FullName;
                existingPatient.Email = patient.Email;
                existingPatient.PhoneNumber = patient.PhoneNumber;
                existingPatient.DateOfBirth = patient.DateOfBirth;
                existingPatient.Gender = patient.Gender;
                existingPatient.Address = patient.Address;

                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        [HttpPost]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return Json(new { success = false, message = "Không tìm thấy bệnh nhân" });
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetPatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return Json(new { success = false, message = "Không tìm thấy bệnh nhân" });
            }
            return Json(new {
                success = true,
                id = patient.Id,
                fullName = patient.FullName,
                email = patient.Email,
                phoneNumber = patient.PhoneNumber,
                dateOfBirth = patient.DateOfBirth.ToString("yyyy-MM-dd"),
                gender = patient.Gender,
                address = patient.Address
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateService(Service service)
        {
            if (ModelState.IsValid)
            {
                service.CreatedAt = DateTime.Now;
                service.IsActive = true;
                _context.Services.Add(service);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
        }

        [HttpGet]
        public async Task<IActionResult> GetService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return Json(new { success = false, message = "Không tìm thấy dịch vụ" });
            }
            return Json(new {
                success = true,
                id = service.Id,
                name = service.Name,
                description = service.Description,
                price = service.Price,
                duration = service.Duration.TotalMinutes,
                category = service.Category,
                isActive = service.IsActive
            });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateService(Service service)
        {
            if (ModelState.IsValid)
            {
                var existingService = await _context.Services.FindAsync(service.Id);
                if (existingService == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy dịch vụ" });
                }

                existingService.Name = service.Name;
                existingService.Description = service.Description;
                existingService.Price = service.Price;
                existingService.Duration = TimeSpan.FromMinutes(service.Duration.TotalMinutes);
                existingService.Category = service.Category;
                existingService.IsActive = service.IsActive;
                existingService.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Dữ liệu không hợp lệ" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return Json(new { success = false, message = "Không tìm thấy dịch vụ" });
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}