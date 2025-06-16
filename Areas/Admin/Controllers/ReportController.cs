using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyMvcApp.Models;
using MyMvcApp.Services.Interfaces;
using MyMvcApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using MyMvcApp.ViewModels;

namespace MyMvcApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public ReportController(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RevenueReport(DateTime? startDate, DateTime? endDate)
        {
            startDate ??= DateTime.Today.AddMonths(-1);
            endDate ??= DateTime.Today;

            var revenueData = _context.Payments
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .GroupBy(p => p.PaymentDate.Date)
                .Select(g => new RevenueReportViewModel
                {
                    Date = g.Key,
                    TotalAmount = g.Sum(p => p.Amount)
                })
                .OrderBy(r => r.Date)
                .ToList();

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            return View(revenueData);
        }

        public IActionResult ServiceReport(DateTime? startDate, DateTime? endDate)
        {
            startDate ??= DateTime.Today.AddMonths(-1);
            endDate ??= DateTime.Today;

            var serviceData = _context.Payments
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .GroupBy(p => p.Service.Name)
                .Select(g => new ServiceReportViewModel
                {
                    ServiceName = g.Key,
                    TotalCount = g.Count(),
                    TotalRevenue = g.Sum(p => p.Amount)
                })
                .OrderByDescending(s => s.TotalRevenue)
                .ToList();

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            return View(serviceData);
        }

        public IActionResult PatientReport()
        {
            var patientData = _context.Patients
                .Select(p => new PatientReportViewModel
                {
                    PatientName = p.FullName,
                    TotalAppointments = p.Appointments.Count,
                    TotalPayments = p.Payments.Sum(pay => pay.Amount),
                    LastVisit = p.Appointments
                        .OrderByDescending(a => a.AppointmentDate)
                        .Select(a => (DateTime?)a.AppointmentDate)
                        .FirstOrDefault()
                })
                .OrderByDescending(p => p.TotalPayments)
                .ToList();

            return View(patientData);
        }

        public IActionResult StaffReport(DateTime? startDate, DateTime? endDate)
        {
            startDate ??= DateTime.Today.AddMonths(-1);
            endDate ??= DateTime.Today;

            var staffData = _context.Activities
                .Where(a => a.Time >= startDate && a.Time <= endDate)
                .GroupBy(a => a.User.UserName)
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
