using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MyMvcApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize] // Assuming this page requires authentication
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
