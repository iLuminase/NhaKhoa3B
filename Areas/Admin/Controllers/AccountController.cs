using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using MyMvcApp.Areas.Admin.Models;

namespace MyMvcApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailSender<ApplicationUser> _emailSender;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<AccountController> logger,
            IEmailSender<ApplicationUser> emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign role based on user type
                    await _userManager.AddToRoleAsync(user, model.Role);
                    return RedirectToAction("Index", "Admin");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    // 1. Pre-check if the user is already locked out.
                    if (user != null && await _userManager.IsLockedOutAsync(user))
                    {
                        _logger.LogWarning($"Login attempt for an already locked out account: {model.Email}");
                        ModelState.AddModelError(string.Empty, "Tài khoản này hiện đang bị khóa. Vui lòng thử lại sau 24 giờ.");
                        return View(model);
                    }

                    // 2. Attempt to sign in.
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

                    if (result.Succeeded)
                    {
                        // This should be the same user object from before.
                        var successfulUser = user ?? await _userManager.FindByEmailAsync(model.Email);
                        if (successfulUser == null)
                        {
                            // This is an edge case and should not happen.
                            ModelState.AddModelError(string.Empty, "Tài khoản không tồn tại.");
                            _logger.LogWarning($"Login succeeded but user not found: {model.Email}");
                            return View(model);
                        }

                        // 3. Reset access failed count on successful login.
                        await _userManager.ResetAccessFailedCountAsync(successfulUser);

                        // 4. Check if user is active (original logic).
                        if (!successfulUser.IsActive)
                        {
                            ModelState.AddModelError(string.Empty, "Tài khoản của bạn đã bị vô hiệu hóa. Vui lòng liên hệ quản trị viên.");
                            _logger.LogWarning($"Login attempt with inactive user: {model.Email}");
                            await _signInManager.SignOutAsync(); // Sign out immediately.
                            return View(model);
                        }

                        // 5. Redirect based on role (original logic).
                        if (await _userManager.IsInRoleAsync(successfulUser, "Admin"))
                        {
                            _logger.LogInformation($"Admin user logged in: {model.Email}");
                            return RedirectToAction("Index", "Admin");
                        }
                        else if (await _userManager.IsInRoleAsync(successfulUser, "Dentist") || await _userManager.IsInRoleAsync(successfulUser, "Staff"))
                        {
                            _logger.LogInformation($"Dentist/Staff user logged in: {model.Email}");
                            return RedirectToAction("Index", "User");
                        }
                        _logger.LogInformation($"Regular user logged in: {model.Email}");
                        return RedirectToAction("Index", "Home");
                    }

                    if (result.IsLockedOut)
                    {
                        // This handles the case where the user is locked out on THIS attempt.
                        _logger.LogWarning($"User account just locked out: {model.Email}");
                        ModelState.AddModelError(string.Empty, "Tài khoản của bạn đã bị khóa trong 24 giờ do nhập sai mật khẩu quá nhiều lần.");
                    }
                    else
                    {
                        // Failed attempt, but not locked out yet.
                        if (user != null)
                        {
                            var failedAttempts = await _userManager.GetAccessFailedCountAsync(user);
                            var maxAttempts = _userManager.Options.Lockout.MaxFailedAccessAttempts;
                            var remainingAttempts = maxAttempts - failedAttempts;
                            ModelState.AddModelError(string.Empty, $"Email hoặc mật khẩu không đúng. Bạn còn {remainingAttempts} lần thử trước khi tài khoản bị khóa.");
                        }
                        else
                        {
                            // User not found.
                            ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không đúng.");
                        }
                        _logger.LogWarning($"Failed login attempt for: {model.Email}");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi đăng nhập. Vui lòng thử lại.");
                    _logger.LogError($"Exception during login: {ex.Message}");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Clear the existing external cookie
            await _signInManager.SignOutAsync();

            // Clear all cookies
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            // Clear session
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var model = new ProfileViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email!,
                UserName = user.UserName!,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Roles = roles.ToList(),
                CreatedAt = user.CreatedAt
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            if (model.DateOfBirth.HasValue)
            {
                user.DateOfBirth = model.DateOfBirth.Value;
            }
            user.Gender = model.Gender ?? string.Empty;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Thông tin tài khoản đã được cập nhật thành công.";
                return RedirectToAction(nameof(Profile));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = "Mật khẩu đã được thay đổi thành công.";
                return RedirectToAction(nameof(ChangePassword));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Settings()
        {
            // In a real application, you would load user settings from database
            var model = new SettingsViewModel();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Settings(SettingsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // In a real application, you would save settings to database
            TempData["SuccessMessage"] = "Cài đặt đã được lưu thành công.";
            return RedirectToAction(nameof(Settings));
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CheckLockoutStatus(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { isLocked = false });
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Để bảo mật, không tiết lộ tài khoản không tồn tại
                return Json(new { isLocked = false });
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
                // Chuyển sang giờ địa phương và định dạng cho thân thiện
                var localLockoutEnd = lockoutEnd?.ToLocalTime();
                return Json(new
                {
                    isLocked = true,
                    lockoutEnd = localLockoutEnd?.ToString("HH:mm 'ngày' dd/MM/yyyy")
                });
            }

            return Json(new { isLocked = false });
        }


        [HttpPost]
        [Route("Admin/Account/RegisterPublic")]
        public async Task<IActionResult> RegisterPublic(PublicRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    PhoneNumber = model.PhoneNumber,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Gán role mặc định "Staff" cho user mới đăng ký
                    // Admin có thể thay đổi role sau trong quản lý người dùng
                    await _userManager.AddToRoleAsync(user, "Staff");
                    
                    _logger.LogInformation("User created a new account with password: {Email}", model.Email);

                    // Tự động đăng nhập sau khi đăng ký thành công
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View("RegisterPublic", model);
        }

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}