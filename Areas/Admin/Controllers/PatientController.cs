using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Models;
using MyMvcApp.Services.Interfaces;
using MyMvcApp.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace MyMvcApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly ILogger<PatientController> _logger;

        public PatientController(
            ApplicationDbContext context,
            IUserService userService,
            ILogger<PatientController> logger)
        {
            _context = context;
            _userService = userService;
            _logger = logger;
        }

        // GET: Patient
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userService.GetCurrentUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var patients = await _context.Patients
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(patients);
        }

        // GET: Patient/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    patient.CreatedAt = DateTime.Now;
                    _context.Add(patient);
                    await _context.SaveChangesAsync();

                    // Log activity
                    var currentUser = await _userService.GetCurrentUserAsync(User);
                    if (currentUser != null)
                    {
                        var activity = new Activity
                        {
                            Time = DateTime.Now,
                            Description = $"Tạo bệnh nhân mới: {patient.FullName}",
                            UserId = currentUser.Id,
                            User = currentUser
                        };
                        _context.Activities.Add(activity);
                        await _context.SaveChangesAsync();
                    }

                    TempData["SuccessMessage"] = "Tạo bệnh nhân thành công.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating patient");
                    ModelState.AddModelError("", "Có lỗi xảy ra khi tạo bệnh nhân. Vui lòng thử lại.");
                }
            }
            return View(patient);
        }

        // GET: Patient/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patient/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    patient.UpdatedAt = DateTime.Now;
                    _context.Update(patient);
                    await _context.SaveChangesAsync();

                    // Log activity
                    var currentUser = await _userService.GetCurrentUserAsync(User);
                    if (currentUser != null)
                    {
                        var activity = new Activity
                        {
                            Time = DateTime.Now,
                            Description = $"Cập nhật thông tin bệnh nhân: {patient.FullName}",
                            UserId = currentUser.Id,
                            User = currentUser
                        };
                        _context.Activities.Add(activity);
                        await _context.SaveChangesAsync();
                    }

                    TempData["SuccessMessage"] = "Cập nhật thông tin bệnh nhân thành công.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patient/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);
                if (patient != null)
                {
                    // Check if patient has any appointments
                    var hasAppointments = await _context.Appointments.AnyAsync(a => a.PatientId == id);
                    if (hasAppointments)
                    {
                        TempData["ErrorMessage"] = "Không thể xóa bệnh nhân đã có lịch hẹn.";
                        return RedirectToAction(nameof(Index));
                    }

                    _context.Patients.Remove(patient);
                    await _context.SaveChangesAsync();

                    // Log activity
                    var currentUser = await _userService.GetCurrentUserAsync(User);
                    if (currentUser != null)
                    {
                        var activity = new Activity
                        {
                            Time = DateTime.Now,
                            Description = $"Xóa bệnh nhân: {patient.FullName}",
                            UserId = currentUser.Id,
                            User = currentUser
                        };
                        _context.Activities.Add(activity);
                        await _context.SaveChangesAsync();
                    }

                    TempData["SuccessMessage"] = "Xóa bệnh nhân thành công.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting patient");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa bệnh nhân.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
