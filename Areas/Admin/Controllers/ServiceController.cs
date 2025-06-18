using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Services.Interfaces;
using MyMvcApp.Data;
using System.Threading.Tasks;
using MyMvcApp.Areas.Admin.Models;

namespace MyMvcApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public ServiceController(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public IActionResult Index()
        {
            var services = _context.Services.ToList();
            return View(services);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Service service)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userService.GetCurrentUserAsync(User);
                if (currentUser != null)
                {
                    service.CreatedBy = currentUser.Id;
                    _context.Services.Add(service);
                    await _context.SaveChangesAsync();

                    // Log activity
                    var activity = new Activity
                    {
                        Time = DateTime.Now,
                        Description = $"Created new service: {service.Name}",
                        UserId = currentUser.Id,
                        User = currentUser
                    };
                    _context.Activities.Add(activity);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            return View(service);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Service service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();

                    // Log activity
                    var currentUser = await _userService.GetCurrentUserAsync(User);
                    if (currentUser != null)
                    {
                        var activity = new Activity
                        {
                            Time = DateTime.Now,
                            Description = $"Updated service: {service.Name}",
                            UserId = currentUser.Id,
                            User = currentUser
                        };
                        _context.Activities.Add(activity);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception)
                {
                    if (!ServiceExists(service.Id))
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
            return View(service);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();

                // Log activity
                var currentUser = await _userService.GetCurrentUserAsync(User);
                if (currentUser != null)
                {
                    var activity = new Activity
                    {
                        Time = DateTime.Now,
                        Description = $"Deleted service: {service.Name}",
                        UserId = currentUser.Id,
                        User = currentUser
                    };
                    _context.Activities.Add(activity);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
} 