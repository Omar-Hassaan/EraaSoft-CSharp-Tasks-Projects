using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBookingApp.Data;
using MovieBookingApp.Models;

namespace MovieBookingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var movies = _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .ToList();

            ViewBag.Actors = _context.Actors.ToList();

            return View(movies);
        }
    }
}
