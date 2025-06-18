using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyMvcApp.Data;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Areas.Admin.Models;

namespace MyMvcApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TestDataController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Tạo dữ liệu test cho lịch hẹn đã hoàn thành
        public async Task<IActionResult> CreateCompletedAppointments()
        {
            try
            {
                // Lấy dữ liệu cần thiết
                var patients = await _context.Patients.Take(3).ToListAsync();
                var services = await _context.Services.Take(3).ToListAsync();
                var dentists = await _context.Users.Where(u => u.Email.Contains("dentist")).Take(2).ToListAsync();

                if (!patients.Any() || !services.Any() || !dentists.Any())
                {
                    TempData["ErrorMessage"] = "Cần có ít nhất 3 bệnh nhân, 3 dịch vụ và 2 nha sĩ để tạo dữ liệu test";
                    return RedirectToAction("Index", "Admin");
                }

                var completedAppointments = new List<Appointment>();

                // Tạo 5 lịch hẹn đã hoàn thành
                for (int i = 0; i < 5; i++)
                {
                    var patient = patients[i % patients.Count];
                    var service = services[i % services.Count];
                    var dentist = dentists[i % dentists.Count];

                    var appointment = new Appointment
                    {
                        PatientId = patient.Id,
                        DentistId = dentist.Id,
                        ServiceId = service.Id,
                        AppointmentDate = DateTime.Now.AddDays(-i - 1), // Các ngày trong quá khứ
                        StartTime = new TimeSpan(9 + i, 0, 0), // 9:00, 10:00, 11:00, etc.
                        EndTime = new TimeSpan(9 + i + 1, 0, 0), // 10:00, 11:00, 12:00, etc.
                        Status = "Completed", // Đã hoàn thành
                        Notes = $"Lịch hẹn test số {i + 1} - Đã hoàn thành điều trị",
                        CreatedAt = DateTime.Now.AddDays(-i - 2),
                        CreatedByUserId = dentist.Id,
                        CreatedBy = dentist.FullName,
                        PatientName = patient.FullName
                    };

                    completedAppointments.Add(appointment);
                }

                _context.Appointments.AddRange(completedAppointments);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Đã tạo thành công {completedAppointments.Count} lịch hẹn đã hoàn thành để test thanh toán";
                return RedirectToAction("Index", "Payment");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tạo dữ liệu test: {ex.Message}";
                return RedirectToAction("Index", "Admin");
            }
        }

        // Xóa tất cả dữ liệu test
        public async Task<IActionResult> ClearTestData()
        {
            try
            {
                // Xóa tất cả PaymentTransactions
                var transactions = await _context.PaymentTransactions.ToListAsync();
                _context.PaymentTransactions.RemoveRange(transactions);

                // Xóa các lịch hẹn test (có Notes chứa "test")
                var testAppointments = await _context.Appointments
                    .Where(a => a.Notes != null && a.Notes.Contains("test"))
                    .ToListAsync();
                _context.Appointments.RemoveRange(testAppointments);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Đã xóa {transactions.Count} giao dịch và {testAppointments.Count} lịch hẹn test";
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi xóa dữ liệu test: {ex.Message}";
                return RedirectToAction("Index", "Admin");
            }
        }

        // Tạo một lịch hẹn đã hoàn thành cụ thể
        [HttpPost]
        public async Task<IActionResult> CreateSingleCompletedAppointment(int patientId, int serviceId, string dentistId)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(patientId);
                var service = await _context.Services.FindAsync(serviceId);
                var dentist = await _context.Users.FindAsync(dentistId);

                if (patient == null || service == null || dentist == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy dữ liệu cần thiết" });
                }

                var appointment = new Appointment
                {
                    PatientId = patient.Id,
                    DentistId = dentist.Id,
                    ServiceId = service.Id,
                    AppointmentDate = DateTime.Now.AddDays(-1), // Hôm qua
                    StartTime = new TimeSpan(10, 0, 0), // 10:00
                    EndTime = new TimeSpan(11, 0, 0), // 11:00
                    Status = "Completed",
                    Notes = "Lịch hẹn test - Đã hoàn thành điều trị",
                    CreatedAt = DateTime.Now.AddDays(-2),
                    CreatedByUserId = dentist.Id,
                    CreatedBy = dentist.FullName,
                    PatientName = patient.FullName
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Đã tạo lịch hẹn test thành công", appointmentId = appointment.Id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }
    }
}
