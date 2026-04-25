using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieBookingApp.Data;
using MovieBookingApp.Models;

namespace MovieBookingApp.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? categoryId, int? cinemaId)
        {
            var movies = _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .AsQueryable();

            if (categoryId != null)
                movies = movies.Where(m => m.CategoryId == categoryId);

            if (cinemaId != null)
                movies = movies.Where(m => m.CinemaId == cinemaId);

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Cinemas = _context.Cinemas.ToList();

            return View(movies.ToList());
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.Cinemas = new SelectList(_context.Cinemas, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Movies.Add(movie);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.Cinemas = new SelectList(_context.Cinemas, "Id", "Name");
            return View(movie);
        }

        public IActionResult Edit(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null) return NotFound();

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", movie.CategoryId);
            ViewBag.Cinemas = new SelectList(_context.Cinemas, "Id", "Name", movie.CinemaId);
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Movies.Update(movie);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", movie.CategoryId);
            ViewBag.Cinemas = new SelectList(_context.Cinemas, "Id", "Name", movie.CinemaId);
            return View(movie);
        }

        public IActionResult Details(int id)
        {
            var movie = _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .Include(m => m.Actors)
                .FirstOrDefault(m => m.Id == id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        public IActionResult Delete(int id)
        {
            var movie = _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .FirstOrDefault(m => m.Id == id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Search(string term)
        {
            var result = _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .Where(m => m.Name.Contains(term ?? ""))
                .ToList();

            return PartialView("_MovieCards", result);
        }
    }
}
