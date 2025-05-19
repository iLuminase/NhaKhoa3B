using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyMvcApp.Models;
using MyMvcApp.Services.Interfaces;
using MyMvcApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MyMvcApp.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public PaymentController(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public IActionResult Index()
        {
            var payments = _context.Payments
                .Include(p => p.Patient)
                .Include(p => p.Service)
                .ToList();
            return View(payments);
        }

        public IActionResult Create()
        {
            ViewBag.Patients = _context.Patients.ToList();
            ViewBag.Services = _context.Services.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Payment payment)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userService.GetCurrentUserAsync(User);
                if (currentUser != null)
                {
                    payment.CreatedBy = currentUser.Id;
                    payment.PaymentDate = DateTime.Now;
                    _context.Payments.Add(payment);
                    await _context.SaveChangesAsync();

                    // Log activity
                    var activity = new Activity
                    {
                        Time = DateTime.Now,
                        Description = $"Created new payment for patient: {payment.Patient.Name}",
                        UserId = currentUser.Id,
                        User = currentUser
                    };
                    _context.Activities.Add(activity);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.Patients = _context.Patients.ToList();
            ViewBag.Services = _context.Services.ToList();
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
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
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
                        Description = $"Deleted payment for patient: {payment.Patient.Name}",
                        UserId = currentUser.Id,
                        User = currentUser
                    };
                    _context.Activities.Add(activity);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
} 