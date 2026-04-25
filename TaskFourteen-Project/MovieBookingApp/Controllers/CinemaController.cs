using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBookingApp.Data;
using MovieBookingApp.Models;

namespace MovieBookingApp.Controllers
{
    public class CinemaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CinemaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Cinemas.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Cinema cinema)
        {
            if (ModelState.IsValid)
            {
                _context.Cinemas.Add(cinema);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cinema);
        }

        public IActionResult Edit(int id)
        {
            var cinema = _context.Cinemas.Find(id);
            if (cinema == null) return NotFound();
            return View(cinema);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Cinema cinema)
        {
            if (ModelState.IsValid)
            {
                _context.Cinemas.Update(cinema);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cinema);
        }

        public IActionResult Delete(int id)
        {
            var cinema = _context.Cinemas.Find(id);
            if (cinema == null) return NotFound();
            return View(cinema);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var cinema = _context.Cinemas.Find(id);
            if (cinema != null)
            {
                _context.Cinemas.Remove(cinema);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var cinema = _context.Cinemas
                .Include(c => c.Movies)
                .FirstOrDefault(c => c.Id == id);
            if (cinema == null) return NotFound();
            return View(cinema);
        }
    }
}
