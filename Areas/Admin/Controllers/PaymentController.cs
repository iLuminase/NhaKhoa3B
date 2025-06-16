using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyMvcApp.Models;
using MyMvcApp.Services.Interfaces;
using MyMvcApp.Data;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.ViewModels.Admin;
using System.Threading.Tasks;

namespace MyMvcApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly ILogger<PaymentController> _logger;
        private readonly IMoMoService _momoService;

        public PaymentController(
            ApplicationDbContext context,
            IUserService userService,
            ILogger<PaymentController> logger,
            IMoMoService momoService)
        {
            _context = context;
            _userService = userService;
            _logger = logger;
            _momoService = momoService;
        }

        // Hiển thị danh sách lịch hẹn đã hoàn thành
        public async Task<IActionResult> Index()
        {
            var completedAppointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Service)
                .Include(a => a.Dentist)
                .Where(a => a.Status == "Completed")
                .OrderByDescending(a => a.AppointmentDate) // Order by date first
                .ToListAsync();

            var viewModelList = new List<PaymentAppointmentViewModel>();
            foreach (var app in completedAppointments)
            {
                var isPaid = await _context.PaymentTransactions
                                     .AnyAsync(pt => pt.AppointmentId == app.Id && pt.Status == "Success");
                viewModelList.Add(new PaymentAppointmentViewModel
                {
                    Appointment = app,
                    IsPaid = isPaid,
                    PaymentStatusString = isPaid ? "Đã thanh toán" : "Chờ thanh toán"
                });
            }

            // Sort by payment status (unpaid first), then by appointment date
            var sortedViewModelList = viewModelList
                                        .OrderBy(vm => vm.IsPaid) // false (unpaid) comes before true (paid)
                                        .ThenByDescending(vm => vm.Appointment.AppointmentDate)
                                        .ToList();

            return View(sortedViewModelList);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Patients = await _context.Patients.OrderBy(p => p.FullName).ToListAsync();
            ViewBag.Services = await _context.Services.Where(s => s.IsActive).OrderBy(s => s.Name).ToListAsync();
            ViewBag.Appointments = await _context.Appointments
                .Include(a => a.Patient)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Payment payment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUser = await _userService.GetCurrentUserAsync(User);
                    if (currentUser != null)
                    {
                        payment.CreatedBy = currentUser.Id;
                        payment.PaymentDate = DateTime.Now;
                        payment.CreatedAt = DateTime.Now;

                        _context.Payments.Add(payment);
                        await _context.SaveChangesAsync();

                        // Log activity
                        var activity = new Activity
                        {
                            Time = DateTime.Now,
                            Description = $"Tạo thanh toán mới cho bệnh nhân: {payment.Patient.FullName}, số tiền: {payment.Amount:N0} VNĐ",
                            UserId = currentUser.Id,
                            User = currentUser
                        };
                        _context.Activities.Add(activity);
                        await _context.SaveChangesAsync();

                        TempData["SuccessMessage"] = "Tạo thanh toán thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo thanh toán");
                ModelState.AddModelError("", "Có lỗi xảy ra khi tạo thanh toán. Vui lòng thử lại sau.");
            }

            ViewBag.Patients = await _context.Patients.OrderBy(p => p.FullName).ToListAsync();
            ViewBag.Services = await _context.Services.Where(s => s.IsActive).OrderBy(s => s.Name).ToListAsync();
            ViewBag.Appointments = await _context.Appointments
                .Include(a => a.Patient)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(payment);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Patient)
                .Include(p => p.Service)
                .Include(p => p.Appointment)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
            {
                return NotFound();
            }

            ViewBag.Patients = await _context.Patients.OrderBy(p => p.FullName).ToListAsync();
            ViewBag.Services = await _context.Services.OrderBy(s => s.Name).ToListAsync();
            ViewBag.Appointments = await _context.Appointments
                .Include(a => a.Patient)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Payment payment)
        {
            if (id != payment.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var existingPayment = await _context.Payments.FindAsync(id);
                    if (existingPayment == null)
                    {
                        return NotFound();
                    }

                    // Update properties
                    existingPayment.Amount = payment.Amount;
                    existingPayment.PaymentMethod = payment.PaymentMethod;
                    existingPayment.Status = payment.Status;
                    existingPayment.TransactionId = payment.TransactionId;
                    existingPayment.Notes = payment.Notes;
                    existingPayment.UpdatedAt = DateTime.Now;
                    existingPayment.PatientId = payment.PatientId;
                    existingPayment.ServiceId = payment.ServiceId;
                    existingPayment.AppointmentId = payment.AppointmentId;

                    _context.Update(existingPayment);
                    await _context.SaveChangesAsync();

                    // Log activity
                    var currentUser = await _userService.GetCurrentUserAsync(User);
                    if (currentUser != null)
                    {
                        var activity = new Activity
                        {
                            Time = DateTime.Now,
                            Description = $"Cập nhật thanh toán cho bệnh nhân: {existingPayment.Patient.FullName}, số tiền: {existingPayment.Amount:N0} VNĐ",
                            UserId = currentUser.Id,
                            User = currentUser
                        };
                        _context.Activities.Add(activity);
                        await _context.SaveChangesAsync();
                    }

                    TempData["SuccessMessage"] = "Cập nhật thanh toán thành công!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!PaymentExists(payment.Id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "Lỗi khi cập nhật thanh toán");
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật thanh toán. Vui lòng thử lại sau.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật thanh toán");
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật thanh toán. Vui lòng thử lại sau.");
            }

            ViewBag.Patients = await _context.Patients.OrderBy(p => p.FullName).ToListAsync();
            ViewBag.Services = await _context.Services.OrderBy(s => s.Name).ToListAsync();
            ViewBag.Appointments = await _context.Appointments
                .Include(a => a.Patient)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(payment);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Patient)
                .Include(p => p.Service)
                .Include(p => p.Appointment)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Patient)
                .Include(p => p.Service)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var payment = await _context.Payments
                    .Include(p => p.Patient)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (payment != null)
                {
                    _context.Payments.Remove(payment);
                    await _context.SaveChangesAsync();

                    // Log activity
                    var currentUser = await _userService.GetCurrentUserAsync(User);
                    if (currentUser != null)
                    {
                        var activity = new Activity
                        {
                            Time = DateTime.Now,
                            Description = $"Xóa thanh toán của bệnh nhân: {payment.Patient.FullName}, số tiền: {payment.Amount:N0} VNĐ",
                            UserId = currentUser.Id,
                            User = currentUser
                        };
                        _context.Activities.Add(activity);
                        await _context.SaveChangesAsync();
                    }

                    TempData["SuccessMessage"] = "Xóa thanh toán thành công!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa thanh toán");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa thanh toán. Vui lòng thử lại sau.";
            }

            return RedirectToAction(nameof(Index));
        }

        // Xem chi tiết thanh toán cho lịch hẹn
        public async Task<IActionResult> PaymentDetails(int appointmentId)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Service)
                .Include(a => a.Dentist)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null || appointment.Status != "Completed")
            {
                return NotFound();
            }

            // Kiểm tra xem đã có giao dịch thanh toán nào chưa
            var existingTransaction = await _context.PaymentTransactions
                .FirstOrDefaultAsync(pt => pt.AppointmentId == appointmentId);

            ViewBag.ExistingTransaction = existingTransaction;
            return View(appointment);
        }

        // Tạo thanh toán MoMo
        [HttpPost]
        public async Task<IActionResult> CreateMoMoPayment(int appointmentId)
        {
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Service)
                    .FirstOrDefaultAsync(a => a.Id == appointmentId);

                if (appointment == null || appointment.Status != "Completed")
                {
                    return Json(new { success = false, message = "Lịch hẹn không hợp lệ" });
                }

                var orderInfo = $"Thanh toán dịch vụ {appointment.Service.Name} - Bệnh nhân: {appointment.Patient.FullName}";
                var response = await _momoService.CreatePaymentAsync(appointmentId, appointment.Service.Price, orderInfo);

                if (response.ResultCode == "0")
                {
                    // Log activity
                    var currentUser = await _userService.GetCurrentUserAsync(User);
                    if (currentUser != null)
                    {
                        var activity = new Activity
                        {
                            Time = DateTime.Now,
                            Description = $"Tạo thanh toán MoMo cho lịch hẹn #{appointmentId} - {appointment.Patient.FullName} - {appointment.Service.Price:N0} VNĐ",
                            UserId = currentUser.Id,
                            User = currentUser
                        };
                        _context.Activities.Add(activity);
                        await _context.SaveChangesAsync();
                    }

                    return Json(new {
                        success = true,
                        payUrl = response.PayUrl,
                        qrCodeUrl = response.QrCodeUrl,
                        orderId = response.OrderId,
                        amount = appointment.Service.Price,
                        patientName = appointment.Patient.FullName,
                        serviceName = appointment.Service.Name
                    });
                }
                else
                {
                    return Json(new { success = false, message = response.Message });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo thanh toán MoMo");
                return Json(new { success = false, message = "Có lỗi xảy ra khi tạo thanh toán" });
            }
        }

        // Tạo thanh toán tiền mặt
        [HttpPost]
        public async Task<IActionResult> CreateCashPayment(int appointmentId)
        {
            try
            {
                var appointment = await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Service)
                    .FirstOrDefaultAsync(a => a.Id == appointmentId);

                if (appointment == null || appointment.Status != "Completed")
                {
                    return Json(new { success = false, message = "Lịch hẹn không hợp lệ" });
                }

                var transaction = await _momoService.CreatePaymentTransactionAsync(
                    appointmentId,
                    appointment.Service.Price,
                    "Cash"
                );

                // Cập nhật trạng thái thành công ngay lập tức cho thanh toán tiền mặt
                await _momoService.UpdatePaymentStatusAsync(transaction.OrderId, "Success");

                // Log activity
                var currentUser = await _userService.GetCurrentUserAsync(User);
                if (currentUser != null)
                {
                    var activity = new Activity
                    {
                        Time = DateTime.Now,
                        Description = $"Thanh toán tiền mặt cho lịch hẹn #{appointmentId} - {appointment.Patient.FullName}",
                        UserId = currentUser.Id,
                        User = currentUser
                    };
                    _context.Activities.Add(activity);
                    await _context.SaveChangesAsync();
                }

                return Json(new { success = true, message = "Thanh toán tiền mặt thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo thanh toán tiền mặt");
                return Json(new { success = false, message = "Có lỗi xảy ra khi tạo thanh toán" });
            }
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }
    }
}
