using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Services.Interfaces;
using MyMvcApp.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using MyMvcApp.Areas.Admin.Models;

namespace MyMvcApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
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
                            Description = $"Created new patient: {patient.FullName}",
                            UserId = currentUser.Id,
                            User = currentUser
                        };
                        _context.Activities.Add(activity);
                        await _context.SaveChangesAsync();
                    }

                    TempData["SuccessMessage"] = "Patient created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating patient");
                    ModelState.AddModelError("", "An error occurred while creating the patient.");
                }
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
                    _context.Update(patient);
                    await _context.SaveChangesAsync();

                    // Log activity
                    var currentUser = await _userService.GetCurrentUserAsync(User);
                    if (currentUser != null)
                    {
                        var activity = new Activity
                        {
                            Time = DateTime.Now,
                            Description = $"Updated patient: {patient.FullName}",
                            UserId = currentUser.Id,
                            User = currentUser
                        };
                        _context.Activities.Add(activity);
                        await _context.SaveChangesAsync();
                    }

                    TempData["SuccessMessage"] = "Patient updated successfully.";
                    return RedirectToAction(nameof(Index));
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
                .Include(p => p.Appointments)
                    .ThenInclude(a => a.Service)
                .Include(p => p.Payments)
                    .ThenInclude(p => p.Service)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (patient == null)
            {
                return NotFound();
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
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                try
                {
                    // Kiểm tra xem bệnh nhân có lịch hẹn hoặc thanh toán không
                    var hasAppointments = await _context.Appointments.AnyAsync(a => a.PatientId == id);
                    var hasPayments = await _context.Payments.AnyAsync(p => p.PatientId == id);

                    if (hasAppointments || hasPayments)
                    {
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return Json(new {
                                success = false,
                                errors = new[] { "Không thể xóa bệnh nhân này vì đã có lịch hẹn hoặc thanh toán liên quan." }
                            });
                        }

                        TempData["ErrorMessage"] = "Không thể xóa bệnh nhân này vì đã có lịch hẹn hoặc thanh toán liên quan.";
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
                            Description = $"Đã xóa bệnh nhân: {patient.FullName}",
                            UserId = currentUser.Id,
                            User = currentUser
                        };
                        _context.Activities.Add(activity);
                        await _context.SaveChangesAsync();
                    }

                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = true });
                    }

                    TempData["SuccessMessage"] = "Đã xóa bệnh nhân thành công.";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi xóa bệnh nhân");

                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new {
                            success = false,
                            errors = new[] { "Có lỗi xảy ra khi xóa bệnh nhân. Vui lòng thử lại." }
                        });
                    }

                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa bệnh nhân. Vui lòng thử lại.";
                }
            }
            else
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new {
                        success = false,
                        errors = new[] { "Không tìm thấy bệnh nhân." }
                    });
                }

                TempData["ErrorMessage"] = "Không tìm thấy bệnh nhân.";
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}