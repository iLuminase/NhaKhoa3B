using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyMvcApp.Data;
using MyMvcApp.Models;
using Microsoft.EntityFrameworkCore;

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

        // Tạo dữ liệu test
        public async Task<IActionResult> CreateTestData()
        {
            try
            {
                // Tạo test appointments
                var testAppointments = new List<Appointment>();
                var patients = await _context.Patients.Take(3).ToListAsync();
                var services = await _context.Services.Take(3).ToListAsync();

                if (patients.Any() && services.Any())
                {
                    for (int i = 0; i < 5; i++)
                    {
                        var appointment = new Appointment
                        {
                            PatientId = patients[i % patients.Count].Id,
                            ServiceId = services[i % services.Count].Id,
                            AppointmentDate = DateTime.Today.AddDays(i),
                            StartTime = TimeSpan.FromHours(9 + i),
                            EndTime = TimeSpan.FromHours(10 + i),
                            Status = "Completed",
                            Notes = $"Test appointment {i + 1}",
                            CreatedAt = DateTime.Now,
                            PatientName = patients[i % patients.Count].FullName
                        };
                        testAppointments.Add(appointment);
                    }

                    _context.Appointments.AddRange(testAppointments);
                    await _context.SaveChangesAsync();

                    // Tạo test payment transactions
                    foreach (var appointment in testAppointments)
                    {
                        var transaction = new PaymentTransaction
                        {
                            AppointmentId = appointment.Id,
                            OrderId = $"TEST_{DateTime.Now.Ticks}_{appointment.Id}",
                            Amount = services[0].Price,
                            PaymentMethod = "Test",
                            Status = "Success",
                            CreatedAt = DateTime.Now,
                            CompletedAt = DateTime.Now
                        };
                        _context.PaymentTransactions.Add(transaction);
                    }

                    await _context.SaveChangesAsync();
                }

                TempData["SuccessMessage"] = $"Đã tạo {testAppointments.Count} lịch hẹn test và giao dịch thanh toán";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tạo dữ liệu test: {ex.Message}";
                return RedirectToAction("Index", "Home");
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
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi xóa dữ liệu test: {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
