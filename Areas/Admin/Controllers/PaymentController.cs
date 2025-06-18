using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyMvcApp.Services.Interfaces;
using MyMvcApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using MyMvcApp.Areas.Admin.Models;

namespace MyMvcApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    [Route("Payment")]
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

        // Hiển thị danh sách lịch hẹn đã hoàn thành chưa thanh toán
        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var completedAppointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Service)
                .Include(a => a.Dentist)
                .Where(a => a.Status == "Completed")
                .Where(a => !_context.PaymentTransactions.Any(pt => pt.AppointmentId == a.Id && pt.Status == "Success"))
                .OrderByDescending(a => a.AppointmentDate)
                .ThenByDescending(a => a.EndTime)
                .ToListAsync();

            return View(completedAppointments);
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
        [HttpGet("PaymentDetails")]
        public async Task<IActionResult> PaymentDetailsForAppointment(int appointmentId)
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
        [HttpPost("CreateMoMoPayment")]
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
        [HttpPost("CreateCashPayment")]
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

        // Xử lý callback từ MoMo
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PaymentNotify()
        {
            try
            {
                // Đọc dữ liệu từ MoMo
                var body = await new StreamReader(Request.Body).ReadToEndAsync();
                _logger.LogInformation($"MoMo callback: {body}");

                // Xử lý callback từ MoMo ở đây
                // Trong demo này, chúng ta sẽ giả lập việc xử lý thành công

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xử lý callback MoMo");
                return BadRequest();
            }
        }

        // Trang trả về sau khi thanh toán
        public async Task<IActionResult> PaymentReturn(string orderId, string resultCode)
        {
            var transaction = await _momoService.GetPaymentTransactionAsync(orderId);

            if (transaction != null)
            {
                if (resultCode == "0")
                {
                    await _momoService.UpdatePaymentStatusAsync(orderId, "Success");
                    TempData["SuccessMessage"] = "Thanh toán thành công!";
                }
                else
                {
                    await _momoService.UpdatePaymentStatusAsync(orderId, "Failed");
                    TempData["ErrorMessage"] = "Thanh toán thất bại!";
                }
            }

            return RedirectToAction("Index");
        }

        // Xuất hóa đơn
        public async Task<IActionResult> GenerateInvoice(int appointmentId)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Service)
                .Include(a => a.Dentist)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                return NotFound();
            }

            var transaction = await _context.PaymentTransactions
                .FirstOrDefaultAsync(pt => pt.AppointmentId == appointmentId && pt.Status == "Success");

            if (transaction == null)
            {
                TempData["ErrorMessage"] = "Chưa có thanh toán thành công cho lịch hẹn này";
                return RedirectToAction("PaymentDetailsForAppointment", new { appointmentId });
            }

            // Thêm thông tin transaction vào ViewBag
            ViewBag.Transaction = transaction;

            // Log activity
            var currentUser = await _userService.GetCurrentUserAsync(User);
            if (currentUser != null)
            {
                var activity = new Activity
                {
                    Time = DateTime.Now,
                    Description = $"Xuất hóa đơn cho lịch hẹn #{appointmentId} - {appointment.Patient.FullName}",
                    UserId = currentUser.Id,
                    User = currentUser
                };
                _context.Activities.Add(activity);
                await _context.SaveChangesAsync();
            }

            return View("Invoice", appointment);
        }

        private string GenerateInvoiceHtml(Appointment appointment, PaymentTransaction transaction)
        {
            return $@"
                <div style='font-family: Arial, sans-serif; max-width: 800px; margin: 0 auto; padding: 20px;'>
                    <div style='text-align: center; margin-bottom: 30px;'>
                        <h1 style='color: #2c3e50;'>HÓA ĐƠN THANH TOÁN</h1>
                        <p style='color: #7f8c8d;'>Phòng khám nha khoa</p>
                    </div>

                    <div style='border: 1px solid #ddd; padding: 20px; margin-bottom: 20px;'>
                        <h3>Thông tin bệnh nhân:</h3>
                        <p><strong>Họ tên:</strong> {appointment.Patient.FullName}</p>
                        <p><strong>Email:</strong> {appointment.Patient.Email}</p>
                        <p><strong>Số điện thoại:</strong> {appointment.Patient.PhoneNumber}</p>
                    </div>

                    <div style='border: 1px solid #ddd; padding: 20px; margin-bottom: 20px;'>
                        <h3>Thông tin dịch vụ:</h3>
                        <p><strong>Dịch vụ:</strong> {appointment.Service.Name}</p>
                        <p><strong>Bác sĩ:</strong> {appointment.Dentist.FullName}</p>
                        <p><strong>Ngày khám:</strong> {appointment.AppointmentDate:dd/MM/yyyy}</p>
                        <p><strong>Giờ khám:</strong> {appointment.StartTime} - {appointment.EndTime}</p>
                    </div>

                    <div style='border: 1px solid #ddd; padding: 20px; margin-bottom: 20px;'>
                        <h3>Thông tin thanh toán:</h3>
                        <p><strong>Mã giao dịch:</strong> {transaction.OrderId}</p>
                        <p><strong>Phương thức:</strong> {transaction.PaymentMethod}</p>
                        <p><strong>Ngày thanh toán:</strong> {transaction.CompletedAt:dd/MM/yyyy HH:mm}</p>
                        <p><strong>Trạng thái:</strong> Đã thanh toán</p>
                    </div>

                    <div style='text-align: right; font-size: 18px; font-weight: bold; color: #e74c3c;'>
                        <p>Tổng tiền: {appointment.Service.Price:N0} VNĐ</p>
                    </div>

                    <div style='text-align: center; margin-top: 30px; color: #7f8c8d;'>
                        <p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi!</p>
                    </div>
                </div>";
        }

        // Xuất hóa đơn PDF
        public async Task<IActionResult> DownloadInvoicePDF(int appointmentId)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Service)
                .Include(a => a.Dentist)
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
            {
                return NotFound();
            }

            var transaction = await _context.PaymentTransactions
                .FirstOrDefaultAsync(pt => pt.AppointmentId == appointmentId && pt.Status == "Success");

            if (transaction == null)
            {
                TempData["ErrorMessage"] = "Chưa có thanh toán thành công cho lịch hẹn này";
                return RedirectToAction("PaymentDetailsForAppointment", new { appointmentId });
            }

            // Tạo HTML content cho PDF
            var htmlContent = GenerateInvoiceHtmlForPDF(appointment, transaction);

            // Trong thực tế, bạn có thể sử dụng thư viện như iTextSharp hoặc PuppeteerSharp để tạo PDF
            // Ở đây chúng ta sẽ trả về HTML content với header để download

            var fileName = $"HoaDon_{appointment.Id}_{DateTime.Now:yyyyMMdd}.html";
            var contentType = "text/html";
            var content = System.Text.Encoding.UTF8.GetBytes(htmlContent);

            return File(content, contentType, fileName);
        }



        private string GenerateInvoiceHtmlForPDF(Appointment appointment, PaymentTransaction transaction)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Hóa đơn thanh toán</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 20px; }}
        .header {{ text-align: center; margin-bottom: 30px; }}
        .info-section {{ margin-bottom: 20px; }}
        .table {{ width: 100%; border-collapse: collapse; }}
        .table th, .table td {{ border: 1px solid #ddd; padding: 8px; text-align: left; }}
        .table th {{ background-color: #f2f2f2; }}
        .total {{ font-size: 18px; font-weight: bold; text-align: right; margin-top: 20px; }}
        .qr-section {{ text-align: center; margin: 20px 0; }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>HÓA ĐƠN THANH TOÁN</h1>
        <h2>PHÒNG KHÁM NHA KHOA ABC</h2>
        <p>Địa chỉ: 123 Đường ABC, Quận XYZ, TP.HCM</p>
        <p>Điện thoại: (028) 1234 5678</p>
    </div>

    <div class='info-section'>
        <h3>Thông tin bệnh nhân:</h3>
        <p><strong>Họ tên:</strong> {appointment.Patient.FullName}</p>
        <p><strong>Ngày sinh:</strong> {(appointment.Patient.DateOfBirth != DateOnly.MinValue ? appointment.Patient.DateOfBirth.ToString("dd/MM/yyyy") : "Chưa cập nhật")}</p>
        <p><strong>Điện thoại:</strong> {appointment.Patient.PhoneNumber}</p>
        <p><strong>Email:</strong> {appointment.Patient.Email}</p>
    </div>

    <div class='info-section'>
        <h3>Thông tin khám:</h3>
        <p><strong>Ngày khám:</strong> {appointment.AppointmentDate.ToString("dd/MM/yyyy")}</p>
        <p><strong>Bác sĩ:</strong> {appointment.Dentist.FullName}</p>
        <p><strong>Dịch vụ:</strong> {appointment.Service.Name}</p>
    </div>

    <table class='table'>
        <thead>
            <tr>
                <th>Dịch vụ</th>
                <th>Số lượng</th>
                <th>Đơn giá</th>
                <th>Thành tiền</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>{appointment.Service.Name}</td>
                <td>1</td>
                <td>{appointment.Service.Price:N0} VNĐ</td>
                <td>{appointment.Service.Price:N0} VNĐ</td>
            </tr>
        </tbody>
    </table>

    <div class='info-section'>
        <h3>Thông tin giao dịch:</h3>
        <p><strong>Mã giao dịch:</strong> {transaction.OrderId}</p>
        <p><strong>Phương thức thanh toán:</strong> {transaction.PaymentMethod}</p>
        <p><strong>Ngày thanh toán:</strong> {transaction.CompletedAt?.ToString("dd/MM/yyyy HH:mm")}</p>
        <p><strong>Trạng thái:</strong> Đã thanh toán</p>
    </div>

    <div class='total'>
        TỔNG TIỀN: {appointment.Service.Price:N0} VNĐ
    </div>

    <div class='qr-section'>
        <p>Mã QR hóa đơn: INVOICE:{appointment.Id}:{appointment.Patient.FullName}:{appointment.Service.Price}:PAID</p>
    </div>

    <div style='text-align: center; margin-top: 40px;'>
        <p>Cảm ơn quý khách đã sử dụng dịch vụ!</p>
        <p><em>Ngày in: {DateTime.Now:dd/MM/yyyy HH:mm}</em></p>
    </div>
</body>
</html>";
        }

        // Xem chi tiết thanh toán trong History
        [HttpGet("PaymentDetails/{id}")]
        public async Task<IActionResult> PaymentDetails(int id)
        {
            var payment = await _context.PaymentTransactions
                .Include(pt => pt.Appointment)
                    .ThenInclude(a => a.Patient)
                .Include(pt => pt.Appointment)
                    .ThenInclude(a => a.Service)
                .Include(pt => pt.Appointment)
                    .ThenInclude(a => a.Dentist)
                .FirstOrDefaultAsync(pt => pt.Id == id);

            if (payment == null)
            {
                return NotFound();
            }

            return PartialView("_PaymentDetailsPartial", payment);
        }

        // Xem lịch sử thanh toán đã hoàn thành
        [HttpGet("History")]
        public IActionResult History()
        {
            // Dữ liệu sẽ được load bằng AJAX
            return View();
        }

        // API để lấy dữ liệu lịch sử thanh toán cho DataTable
        [HttpGet("GetPaymentHistory")]
        public async Task<IActionResult> GetPaymentHistory(string fromDate = "", string toDate = "", string paymentMethod = "")
        {
            var query = _context.PaymentTransactions
                .Include(pt => pt.Appointment)
                    .ThenInclude(a => a.Patient)
                .Include(pt => pt.Appointment)
                    .ThenInclude(a => a.Service)
                .Include(pt => pt.Appointment)
                    .ThenInclude(a => a.Dentist)
                .Where(pt => pt.Status == "Success")
                .AsQueryable();

            // Filter theo khoảng thời gian
            if (!string.IsNullOrEmpty(fromDate) && DateTime.TryParse(fromDate, out var from))
            {
                query = query.Where(pt => (pt.CompletedAt ?? pt.CreatedAt).Date >= from.Date);
            }

            if (!string.IsNullOrEmpty(toDate) && DateTime.TryParse(toDate, out var to))
            {
                query = query.Where(pt => (pt.CompletedAt ?? pt.CreatedAt).Date <= to.Date);
            }

            // Filter theo phương thức thanh toán
            if (!string.IsNullOrEmpty(paymentMethod))
            {
                query = query.Where(pt => pt.PaymentMethod == paymentMethod);
            }

            var paymentHistory = await query
                .OrderByDescending(pt => pt.CompletedAt ?? pt.CreatedAt)
                .Select(pt => new
                {
                    id = pt.Id,
                    orderId = pt.OrderId,
                    patientName = pt.Appointment.Patient.FullName,
                    serviceName = pt.Appointment.Service.Name,
                    dentistName = pt.Appointment.Dentist.FullName,
                    amount = pt.Amount,
                    paymentMethod = pt.PaymentMethod,
                    appointmentDate = pt.Appointment.AppointmentDate.ToString("yyyy-MM-dd"),
                    appointmentDateDisplay = pt.Appointment.AppointmentDate.ToString("dd/MM/yyyy"),
                    completedAt = (pt.CompletedAt ?? pt.CreatedAt).ToString("yyyy-MM-dd HH:mm:ss"),
                    completedAtDisplay = (pt.CompletedAt ?? pt.CreatedAt).ToString("dd/MM/yyyy HH:mm"),
                    transactionId = pt.TransactionId ?? pt.MoMoTransactionId,
                    notes = pt.Notes
                })
                .ToListAsync();

            return Json(new { data = paymentHistory });
        }

        // In hóa đơn cho payment transaction
        [HttpGet("PrintInvoice/{id}")]
        public async Task<IActionResult> PrintInvoice(int id)
        {
            var payment = await _context.PaymentTransactions
                .Include(pt => pt.Appointment)
                    .ThenInclude(a => a.Patient)
                .Include(pt => pt.Appointment)
                    .ThenInclude(a => a.Service)
                .Include(pt => pt.Appointment)
                    .ThenInclude(a => a.Dentist)
                .FirstOrDefaultAsync(pt => pt.Id == id);

            if (payment == null)
            {
                return NotFound();
            }

            ViewBag.IsForPrint = true;
            return View("PrintInvoice", payment);
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }
    }
}