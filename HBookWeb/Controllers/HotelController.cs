using HBook.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace HBookWeb.Controllers
{
    public class HotelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HotelController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hotels = _context.Hotels.ToList();
            return View();
        }
    }
}
