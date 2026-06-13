using Microsoft.AspNetCore.Mvc;

namespace HBookWeb.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
