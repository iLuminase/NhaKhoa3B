using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MyMvcApp.Controllers
{
    [Authorize] // Assuming this page requires authentication
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
