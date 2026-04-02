using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyMvcApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;
using MyMvcApp.ViewModels;
using System.Globalization;

namespace MyMvcApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? year, int? month)
        {
            // Nếu không có năm tháng, lấy tháng hiện tại
            var targetDate = (year.HasValue && month.HasValue)
                ? new DateTime(year.Value, month.Value, 1)
                : DateTime.Today;

            var firstDayOfMonth = new DateTime(targetDate.Year, targetDate.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            // Lấy dữ liệu thanh toán trong tháng
            var paymentsInMonth = await _context.Payments
                .Where(p => p.PaymentDate >= firstDayOfMonth && p.PaymentDate <= lastDayOfMonth)
                .OrderBy(p => p.PaymentDate)
                .ToListAsync();

            // Xử lý dữ liệu
            var dailyBreakdown = paymentsInMonth
                .GroupBy(p => p.PaymentDate.Day)
                .Select(g => new DailyRevenue
                {
                    Day = g.Key,
                    Revenue = g.Sum(p => p.Amount)
                })
                .ToList();

            var model = new MonthlyRevenueViewModel
            {
                Month = firstDayOfMonth.ToString("MMMM yyyy", new CultureInfo("vi-VN")),
                TotalRevenue = paymentsInMonth.Sum(p => p.Amount),
                TotalAppointments = paymentsInMonth.Select(p => p.AppointmentId).Distinct().Count(),
                DailyBreakdown = dailyBreakdown
            };

            // Dữ liệu cho dropdowns
            ViewBag.Years = await _context.Payments
                .Select(p => p.PaymentDate.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToListAsync();
            ViewBag.Months = Enumerable.Range(1, 12)
                .Select(m => new { Value = m, Name = new DateTime(2000, m, 1).ToString("MMMM", new CultureInfo("vi-VN")) })
                .ToList();

            ViewBag.CurrentYear = targetDate.Year;
            ViewBag.CurrentMonth = targetDate.Month;

            return View(model);
        }
    }
}
                .Select(g => new StaffReportViewModel
                {
                    StaffName = g.Key,
                    TotalActivities = g.Count(),
                    LastActivity = g.Max(a => a.Time)
                })
                .OrderByDescending(s => s.TotalActivities)
                .ToList();

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            return View(staffData);
        }
    }
} 