using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyMvcApp.Services.Interfaces;
using MyMvcApp.Data;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MyMvcApp.Areas.Admin.Models;

namespace MyMvcApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(
            ApplicationDbContext context, 
            IUserService userService,
            UserManager<ApplicationUser> userManager,
            ILogger<AppointmentController> logger)
        {
            _context = context;
            _userService = userService;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Dữ liệu sẽ được load bằng AJAX, không cần pass model
            return View();
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Patients = (await _context.Patients
                .Select(p => new { p.Id, Name = p.FullName })
                .ToListAsync())
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name })
                .ToList();

            ViewBag.Services = (await _context.Services
                .Where(s => s.IsActive)
                .Select(s => new { s.Id, s.Name })
                .ToListAsync())
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                .ToList();

            ViewBag.Dentists = (await _userManager.GetUsersInRoleAsync("Dentist"))
                .Select(d => new SelectListItem { Value = d.Id, Text = d.FullName })
                .ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            try
            {
                // Log the raw request data
                Console.WriteLine("Raw appointment data received:");
                Console.WriteLine($"PatientId: {appointment.PatientId}");
                Console.WriteLine($"DentistId: {appointment.DentistId}");
                Console.WriteLine($"ServiceId: {appointment.ServiceId}");
                Console.WriteLine($"AppointmentDate: {appointment.AppointmentDate}");
                Console.WriteLine($"StartTime: {appointment.StartTime}");
                Console.WriteLine($"EndTime: {appointment.EndTime}");
                Console.WriteLine($"Notes: {appointment.Notes}");

                // Validate required fields
                if (appointment.PatientId <= 0)
                {
                    return Json(new { success = false, errors = new[] { "Vui lòng chọn bệnh nhân." } });
                }
                if (appointment.ServiceId <= 0)
                {
                    return Json(new { success = false, errors = new[] { "Vui lòng chọn dịch vụ." } });
                }
                if (string.IsNullOrEmpty(appointment.DentistId))
                {
                    return Json(new { success = false, errors = new[] { "Vui lòng chọn nha sĩ." } });
                }

                var currentUser = await _userService.GetCurrentUserAsync(User);
                if (currentUser == null)
                {
                    return Json(new { success = false, errors = new[] { "Không tìm thấy thông tin người dùng hiện tại." } });
                }

                // Parse and validate date
                if (appointment.AppointmentDate == default)
                {
                    return Json(new { success = false, errors = new[] { "Ngày hẹn không hợp lệ." } });
                }

                // Ensure the appointment date is in local time
                if (appointment.AppointmentDate.Kind == DateTimeKind.Utc)
                {
                    appointment.AppointmentDate = appointment.AppointmentDate.ToLocalTime();
                }

                // Validate appointment date and time
                if (appointment.AppointmentDate.Date < DateTime.Today)
                {
                    return Json(new { success = false, errors = new[] { "Ngày hẹn không thể là ngày trong quá khứ." } });
                }

                // Parse and validate time
                if (appointment.StartTime == default)
                {
                    return Json(new { success = false, errors = new[] { "Thời gian bắt đầu không hợp lệ." } });
                }
                if (appointment.EndTime == default)
                {
                    return Json(new { success = false, errors = new[] { "Thời gian kết thúc không hợp lệ." } });
                }

                // Validate time range
                if (appointment.StartTime >= appointment.EndTime)
                {
                    return Json(new { success = false, errors = new[] { "Thời gian kết thúc phải sau thời gian bắt đầu." } });
                }

                // Load and validate related entities first
                var patient = await _context.Patients.FindAsync(appointment.PatientId);
                var service = await _context.Services.FindAsync(appointment.ServiceId);
                var dentist = await _userManager.FindByIdAsync(appointment.DentistId);

                if (patient == null)
                {
                    return Json(new { success = false, errors = new[] { "Không tìm thấy thông tin bệnh nhân." } });
                }
                if (service == null)
                {
                    return Json(new { success = false, errors = new[] { "Không tìm thấy thông tin dịch vụ." } });
                }
                if (dentist == null)
                {
                    return Json(new { success = false, errors = new[] { "Không tìm thấy thông tin nha sĩ." } });
                }

                // Check for overlapping appointments
                var overlappingAppointment = await _context.Appointments
                    .Where(a => a.DentistId == appointment.DentistId 
                        && a.AppointmentDate.Date == appointment.AppointmentDate.Date
                        && a.Status != "Cancelled"
                        && (a.StartTime <= appointment.StartTime && a.EndTime > appointment.StartTime
                            || a.StartTime < appointment.EndTime && a.EndTime >= appointment.EndTime
                            || a.StartTime >= appointment.StartTime && a.EndTime <= appointment.EndTime))
                    .FirstOrDefaultAsync();

                if (overlappingAppointment != null)
                {
                    return Json(new { success = false, errors = new[] { "Thời gian này đã có lịch hẹn khác." } });
                }

                // Create new appointment object to avoid any potential issues with the received data
                var newAppointment = new Appointment
                {
                    PatientId = appointment.PatientId,
                    DentistId = appointment.DentistId,
                    ServiceId = appointment.ServiceId,
                    AppointmentDate = appointment.AppointmentDate.Date,
                    StartTime = appointment.StartTime,
                    EndTime = appointment.EndTime,
                    Notes = appointment.Notes,
                    Status = "Scheduled",
                    CreatedAt = DateTime.Now,
                    CreatedByUserId = currentUser.Id,
                    CreatedBy = currentUser.FullName,
                    PatientName = patient.FullName,
                    Patient = patient,
                    Service = service,
                    Dentist = dentist
                };

                // Log the final appointment data before saving
                Console.WriteLine("Final appointment data before saving:");
                Console.WriteLine($"PatientId: {newAppointment.PatientId}");
                Console.WriteLine($"DentistId: {newAppointment.DentistId}");
                Console.WriteLine($"ServiceId: {newAppointment.ServiceId}");
                Console.WriteLine($"AppointmentDate: {newAppointment.AppointmentDate}");
                Console.WriteLine($"StartTime: {newAppointment.StartTime}");
                Console.WriteLine($"EndTime: {newAppointment.EndTime}");
                Console.WriteLine($"Status: {newAppointment.Status}");
                Console.WriteLine($"CreatedByUserId: {newAppointment.CreatedByUserId}");
                Console.WriteLine($"CreatedAt: {newAppointment.CreatedAt}");

                // Add the appointment to the context
                _context.Appointments.Add(newAppointment);

                // Log activity
                var activity = new Activity
                {
                    Time = DateTime.Now,
                    Description = $"Đã tạo lịch hẹn cho bệnh nhân: {patient.FullName}",
                    UserId = currentUser.Id,
                    User = currentUser
                };
                _context.Activities.Add(activity);

                try
                {
                    // Save changes to database
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    // Log the inner exception details
                    var innerException = ex.InnerException;
                    while (innerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {innerException.Message}");
                        Console.WriteLine($"Stack Trace: {innerException.StackTrace}");
                        innerException = innerException.InnerException;
                    }
                    
                    return Json(new { success = false, errors = new[] { "Có lỗi xảy ra khi lưu lịch hẹn. Vui lòng kiểm tra lại thông tin." } });
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Error creating appointment: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                return Json(new { success = false, errors = new[] { "Có lỗi xảy ra khi tạo lịch hẹn: " + ex.Message } });
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Service)
                .Include(a => a.Dentist)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            ViewBag.Patients = (await _context.Patients
                .Select(p => new { p.Id, Name = p.FullName })
                .ToListAsync())
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name })
                .ToList();
            ViewBag.Services = (await _context.Services
                .Where(s => s.IsActive)
                .Select(s => new { s.Id, s.Name })
                .ToListAsync())
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                .ToList();
            ViewBag.Dentists = (await _userManager.GetUsersInRoleAsync("Dentist"))
                .Select(d => new SelectListItem { Value = d.Id, Text = d.FullName })
                .ToList();
            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = await _userService.GetCurrentUserAsync(User);
                    if (currentUser == null)
                    {
                        return Json(new { success = false, errors = new[] { "Không tìm thấy thông tin người dùng hiện tại." } });
                    }

                    // Load related entities
                    var patient = await _context.Patients.FindAsync(appointment.PatientId);
                    var service = await _context.Services.FindAsync(appointment.ServiceId);
                    var dentist = await _userManager.FindByIdAsync(appointment.DentistId);

                    if (patient == null || service == null || dentist == null)
                    {
                        return Json(new { success = false, errors = new[] { "Không tìm thấy thông tin liên quan." } });
                    }

                    // Update appointment
                    appointment.PatientName = patient.FullName;
                    appointment.UpdatedAt = DateTime.Now;
                    appointment.Patient = patient;
                    appointment.Service = service;
                    appointment.Dentist = dentist;

                    _context.Update(appointment);

                    // Log activity
                    var activity = new Activity
                    {
                        Time = DateTime.Now,
                        Description = $"Đã cập nhật lịch hẹn cho bệnh nhân: {appointment.PatientName}",
                        UserId = currentUser.Id,
                        User = currentUser
                    };
                    _context.Activities.Add(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Calendar));
            }

            // Reload ViewBag data if model is invalid
            ViewBag.Patients = (await _context.Patients
                .Select(p => new { p.Id, Name = p.FullName })
                .ToListAsync())
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name })
                .ToList();
            ViewBag.Services = (await _context.Services
                .Where(s => s.IsActive)
                .Select(s => new { s.Id, s.Name })
                .ToListAsync())
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                .ToList();
            ViewBag.Dentists = (await _userManager.GetUsersInRoleAsync("Dentist"))
                .Select(d => new SelectListItem { Value = d.Id, Text = d.FullName })
                .ToList();
            return View(appointment);
        }

        public async Task<IActionResult> Details(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Service)
                .Include(a => a.Dentist)
                .Include(a => a.CreatedByUser)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(id);
                if (appointment == null)
                {
                    return Json(new { success = false, errors = new[] { "Không tìm thấy lịch hẹn" } });
                }

                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting appointment");
                return Json(new { success = false, errors = new[] { "Có lỗi xảy ra khi xóa lịch hẹn" } });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetServiceDuration(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return Json(service.Duration.TotalMinutes);
        }

        public async Task<IActionResult> Calendar()
        {
            // Load ViewBag data for the modal form
            ViewBag.Patients = (await _context.Patients
                .Select(p => new { p.Id, Name = p.FullName })
                .ToListAsync())
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name })
                .ToList();

            ViewBag.Services = (await _context.Services
                .Where(s => s.IsActive)
                .Select(s => new { s.Id, s.Name })
                .ToListAsync())
                .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Name })
                .ToList();

            ViewBag.Dentists = (await _userManager.GetUsersInRoleAsync("Dentist"))
                .Select(d => new SelectListItem { Value = d.Id, Text = d.FullName })
                .ToList();

            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Service)
                .Include(a => a.Dentist)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.StartTime)
                .ToListAsync();

            return View(appointments);
        }

        // API debug để kiểm tra dữ liệu lịch hẹn
        [HttpGet("DebugAppointments")]
        public async Task<IActionResult> DebugAppointments(DateTime? date = null)
        {
            IQueryable<Appointment> query = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Service)
                .Include(a => a.Dentist);

            if (date.HasValue)
            {
                var targetDate = date.Value.Date;
                query = query.Where(a => a.AppointmentDate.Date == targetDate);
            }

            var appointments = await query
                .OrderByDescending(a => a.AppointmentDate)
                .ThenByDescending(a => a.StartTime)
                .ToListAsync();

            var result = appointments.Select(a => new
            {
                id = a.Id,
                patientName = a.Patient.FullName,
                serviceName = a.Service.Name,
                dentistName = a.Dentist.FullName,
                appointmentDate = a.AppointmentDate.ToString("yyyy-MM-dd"),
                startTime = a.StartTime.ToString(@"hh\:mm"),
                endTime = a.EndTime.ToString(@"hh\:mm"),
                status = a.Status,
                notes = a.Notes,
                createdAt = a.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList();

            return Json(new { 
                total = result.Count, 
                filterDate = date?.ToString("yyyy-MM-dd") ?? "All dates",
                data = result 
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Service)
                .Include(a => a.Dentist)
                .Where(a => a.Status != "Cancelled") // Only show active appointments
                .ToListAsync();

            // Hàm tạo màu từ Id và thời gian tạo
            string GetColor(int id, DateTime createdAt)
            {
                // Sử dụng hash của id và thời gian tạo để tạo màu
                var hash = (id + createdAt.Ticks).GetHashCode();
                var hue = (hash % 360 + 360) % 360; // Đảm bảo giá trị từ 0-359
                return $"hsl({hue}, 70%, 50%)"; // Sử dụng HSL để có màu đẹp hơn
            }

            var result = appointments.Select(a => new
            {
                id = a.Id,
                title = $"{a.Patient.FullName} - {a.Service.Name}",
                start = a.AppointmentDate.Date.Add(a.StartTime),
                end = a.AppointmentDate.Date.Add(a.EndTime),
                status = a.Status,
                patientName = a.Patient.FullName,
                serviceName = a.Service.Name,
                dentistName = a.Dentist.FullName,
                notes = a.Notes,
                backgroundColor = GetColor(a.Id, a.CreatedAt),
                borderColor = GetColor(a.Id, a.CreatedAt)
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsCompleted(int id)
        {
            try
            {
                var appointment = await _context.Appointments.FindAsync(id);
                if (appointment == null)
                {
                    return Json(new { success = false, errors = new[] { "Không tìm thấy lịch hẹn" } });
                }

                var currentUser = await _userService.GetCurrentUserAsync(User);
                if (currentUser == null)
                {
                    return Json(new { success = false, errors = new[] { "Không tìm thấy thông tin người dùng hiện tại" } });
                }

                appointment.Status = "Completed";
                appointment.UpdatedAt = DateTime.Now;
                appointment.CreatedByUserId = currentUser.Id;
                appointment.CreatedBy = currentUser.FullName;

                // Log activity with completion information
                var activity = new Activity
                {
                    Time = DateTime.Now,
                    Description = $"Đánh dấu hoàn thành lịch hẹn cho bệnh nhân: {appointment.PatientName}",
                    UserId = currentUser.Id,
                    User = currentUser
                };
                _context.Activities.Add(activity);

                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking appointment as completed");
                return Json(new { success = false, errors = new[] { "Có lỗi xảy ra khi cập nhật trạng thái" } });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointmentsForManagement(string status = "", string date = "")
        {
            var query = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Service)
                .Include(a => a.Dentist)
                .AsQueryable();

            // Filter theo status nếu có
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(a => a.Status == status);
            }
            else
            {
                // Mặc định không hiển thị lịch hẹn đã hủy
                query = query.Where(a => a.Status != "Cancelled");
            }

            // Filter theo ngày nếu có
            if (!string.IsNullOrEmpty(date) && DateTime.TryParse(date, out var filterDate))
            {
                query = query.Where(a => a.AppointmentDate.Date == filterDate.Date);
            }

            var appointments = await query
                .OrderByDescending(a => a.AppointmentDate)
                .ThenByDescending(a => a.StartTime)
                .ToListAsync();

            var result = appointments.Select(a => new
            {
                id = a.Id,
                patientName = a.PatientName,
                serviceName = a.Service.Name,
                appointmentDate = a.AppointmentDate.ToString("dd/MM/yyyy"),
                appointmentDateSort = a.AppointmentDate.ToString("yyyy-MM-dd"), // Cho việc sắp xếp
                startTime = a.StartTime.ToString(@"hh\:mm"),
                status = a.Status,
                statusBadge = a.Status switch
                {
                    "Scheduled" => "bg-primary",
                    "Completed" => "bg-success", 
                    "Cancelled" => "bg-danger",
                    _ => "bg-warning"
                },
                notes = a.Notes?.Length > 50 ? a.Notes.Substring(0, 50) + "..." : a.Notes
            }).ToList();

            return Json(new { data = result });
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
