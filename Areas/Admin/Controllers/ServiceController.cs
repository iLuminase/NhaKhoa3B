using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Models;
using MyMvcApp.Services.Interfaces;
using MyMvcApp.Data;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Index()
        {
            var services = await _context.Services
                .Include(s => s.ServiceCategory)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            // Load categories for dropdown
            ViewBag.ServiceCategories = await _context.ServiceCategories
                .Where(c => c.IsActive)
                .Select(c => new { c.Id, c.Name })
                .ToListAsync();

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

        // Service CRUD Actions for AJAX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateService(Service service, int durationMinutes, string isActiveParam = null)
        {
            try
            {
                // Debug logging
                System.Diagnostics.Debug.WriteLine($"CreateService called with Name: {service.Name}, Duration: {durationMinutes}, IsActive param: {isActiveParam}");
                // Validate duration
                if (durationMinutes <= 0)
                {
                    return Json(new { success = false, message = "Thời gian thực hiện phải lớn hơn 0 phút", errors = new[] { "Thời gian thực hiện phải lớn hơn 0 phút" } });
                }

                // Validate ServiceCategoryId
                if (!service.ServiceCategoryId.HasValue || service.ServiceCategoryId <= 0)
                {
                    return Json(new { success = false, message = "Vui lòng chọn danh mục dịch vụ", errors = new[] { "Vui lòng chọn danh mục dịch vụ" } });
                }

                // Convert duration from minutes to TimeSpan
                service.Duration = TimeSpan.FromMinutes(durationMinutes);

                // Handle IsActive checkbox value (default to true for new services)
                service.IsActive = string.IsNullOrEmpty(isActiveParam) ? true : (!string.IsNullOrEmpty(isActiveParam) && (isActiveParam.ToLower() == "true" || isActiveParam.ToLower() == "on"));

                // Remove Duration and IsActive validation errors since we handle them manually
                ModelState.Remove("Duration");
                ModelState.Remove("IsActive");

                // Auto-fill Category from ServiceCategory if selected
                if (service.ServiceCategoryId.HasValue && service.ServiceCategoryId > 0)
                {
                    var selectedCategory = await _context.ServiceCategories.FindAsync(service.ServiceCategoryId.Value);
                    if (selectedCategory != null)
                    {
                        service.Category = selectedCategory.Name;
                    }
                }

                // If no ServiceCategory selected but Category text is provided, use it
                if (string.IsNullOrEmpty(service.Category))
                {
                    service.Category = "Chưa phân loại"; // Default category
                }

                // Set default values
                service.CreatedAt = DateTime.Now;
                service.IsActive = true;

                if (ModelState.IsValid)
                {
                    _context.Services.Add(service);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }

                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Dữ liệu không hợp lệ", errors = errors });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return Json(new { success = false, message = "Không tìm thấy dịch vụ" });
            }
            return Json(new {
                success = true,
                id = service.Id,
                name = service.Name,
                description = service.Description,
                price = service.Price,
                duration = service.Duration.TotalMinutes,
                category = service.Category,
                serviceCategoryId = service.ServiceCategoryId,
                isActive = service.IsActive
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateService(Service service, int durationMinutes, string isActiveParam = null)
        {
            try
            {
                // Debug logging
                System.Diagnostics.Debug.WriteLine($"UpdateService called with Id: {service.Id}, Name: {service.Name}, Duration: {durationMinutes}, IsActive param: {isActiveParam}");
                // Validate duration
                if (durationMinutes <= 0)
                {
                    return Json(new { success = false, message = "Thời gian thực hiện phải lớn hơn 0 phút", errors = new[] { "Thời gian thực hiện phải lớn hơn 0 phút" } });
                }

                // Validate ServiceCategoryId
                if (!service.ServiceCategoryId.HasValue || service.ServiceCategoryId <= 0)
                {
                    return Json(new { success = false, message = "Vui lòng chọn danh mục dịch vụ", errors = new[] { "Vui lòng chọn danh mục dịch vụ" } });
                }

                var existingService = await _context.Services.FindAsync(service.Id);
                if (existingService == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy dịch vụ" });
                }

                // Handle IsActive checkbox value
                service.IsActive = !string.IsNullOrEmpty(isActiveParam) && (isActiveParam.ToLower() == "true" || isActiveParam.ToLower() == "on");

                // Remove Duration and IsActive validation errors since we handle them manually
                ModelState.Remove("Duration");
                ModelState.Remove("IsActive");

                // Auto-fill Category from ServiceCategory if selected
                if (service.ServiceCategoryId.HasValue && service.ServiceCategoryId > 0)
                {
                    var selectedCategory = await _context.ServiceCategories.FindAsync(service.ServiceCategoryId.Value);
                    if (selectedCategory != null)
                    {
                        service.Category = selectedCategory.Name;
                    }
                }

                // If no ServiceCategory selected but Category text is provided, use it
                if (string.IsNullOrEmpty(service.Category))
                {
                    service.Category = "Chưa phân loại"; // Default category
                }

                if (ModelState.IsValid)
                {
                    existingService.Name = service.Name;
                    existingService.Description = service.Description;
                    existingService.Price = service.Price;
                    existingService.Duration = TimeSpan.FromMinutes(durationMinutes);
                    existingService.Category = service.Category;
                    existingService.ServiceCategoryId = service.ServiceCategoryId;
                    existingService.IsActive = service.IsActive;
                    existingService.UpdatedAt = DateTime.Now;

                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }

                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Dữ liệu không hợp lệ", errors = errors });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteService(int id)
        {
            try
            {
                var service = await _context.Services.FindAsync(id);
                if (service == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy dịch vụ" });
                }

                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        // ServiceCategory Management
        public async Task<IActionResult> Categories()
        {
            var categories = await _context.ServiceCategories
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateServiceCategory(ServiceCategory category)
        {
            try
            {
                // Debug logging
                System.Diagnostics.Debug.WriteLine($"CreateServiceCategory called with Name: {category.Name}");
                if (ModelState.IsValid)
                {
                    category.CreatedAt = DateTime.Now;
                    _context.ServiceCategories.Add(category);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }

                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Dữ liệu không hợp lệ", errors = errors });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetServiceCategory(int id)
        {
            var category = await _context.ServiceCategories.FindAsync(id);
            if (category == null)
            {
                return Json(new { success = false, message = "Không tìm thấy danh mục" });
            }
            return Json(new {
                success = true,
                id = category.Id,
                name = category.Name,
                description = category.Description,
                isActive = category.IsActive
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateServiceCategory(ServiceCategory category, string isActive = null)
        {
            try
            {
                // Debug logging
                System.Diagnostics.Debug.WriteLine($"UpdateServiceCategory called with Id: {category.Id}, Name: {category.Name}, IsActive param: {isActive}");

                if (category.Id <= 0)
                {
                    return Json(new { success = false, message = "ID danh mục không hợp lệ" });
                }

                var existingCategory = await _context.ServiceCategories.FindAsync(category.Id);
                if (existingCategory == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy danh mục với ID: " + category.Id });
                }

                // Handle IsActive checkbox value
                category.IsActive = !string.IsNullOrEmpty(isActive) && (isActive.ToLower() == "true" || isActive.ToLower() == "on");

                // Remove IsActive validation error since we handle it manually
                ModelState.Remove("IsActive");

                // Validate required fields manually
                if (string.IsNullOrWhiteSpace(category.Name))
                {
                    return Json(new { success = false, message = "Tên danh mục là bắt buộc" });
                }

                if (ModelState.IsValid)
                {
                    existingCategory.Name = category.Name.Trim();
                    existingCategory.Description = category.Description?.Trim() ?? "";
                    existingCategory.IsActive = category.IsActive;
                    existingCategory.UpdatedAt = DateTime.Now;

                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }

                // Debug: Log all validation errors
                var allErrors = new List<string>();
                foreach (var modelError in ModelState)
                {
                    foreach (var error in modelError.Value.Errors)
                    {
                        allErrors.Add($"{modelError.Key}: {error.ErrorMessage}");
                    }
                }

                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Dữ liệu không hợp lệ", errors = errors, debugErrors = allErrors });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateServiceCategory error: {ex.Message}");
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteServiceCategory(int id)
        {
            try
            {
                var category = await _context.ServiceCategories.FindAsync(id);
                if (category == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy danh mục" });
                }

                // Check if category is being used by any services
                var servicesUsingCategory = await _context.Services.AnyAsync(s => s.ServiceCategoryId == id);
                if (servicesUsingCategory)
                {
                    return Json(new { success = false, message = "Không thể xóa danh mục đang được sử dụng bởi các dịch vụ" });
                }

                _context.ServiceCategories.Remove(category);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }
    }
}
