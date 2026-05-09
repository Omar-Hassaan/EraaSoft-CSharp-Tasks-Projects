using Microsoft.AspNetCore.Mvc;
using MovieBookingApp.Interfaces;
using MovieBookingApp.Models;

namespace MovieBookingApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActorController : Controller
    {
        private readonly IActorRepository _actorRepo;

        public ActorController(IActorRepository actorRepo) => _actorRepo = actorRepo;

        public IActionResult Index() => View(_actorRepo.GetAll());

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Actor actor)
        {
            if (ModelState.IsValid)
            {
                _actorRepo.Add(actor);
                TempData["Success"] = "Actor added!";
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }

        public IActionResult Edit(int id)
        {
            var actor = _actorRepo.GetById(id);
            if (actor == null) return NotFound();
            return View(actor);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Actor actor)
        {
            if (ModelState.IsValid)
            {
                _actorRepo.Update(actor);
                TempData["Success"] = "Actor updated!";
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }

        public IActionResult Details(int id)
        {
            var actor = _actorRepo.GetByIdWithMovies(id);
            if (actor == null) return NotFound();
            return View(actor);
        }

        public IActionResult Delete(int id)
        {
            var actor = _actorRepo.GetById(id);
            if (actor == null) return NotFound();
            return View(actor);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _actorRepo.Delete(id);
            TempData["Success"] = "Actor deleted!";
            return RedirectToAction(nameof(Index));
        }
    }

    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository categoryRepo) => _categoryRepo = categoryRepo;

        public IActionResult Index() => View(_categoryRepo.GetAll());
        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Add(category);
                TempData["Success"] = "Category added!";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public IActionResult Edit(int id)
        {
            var cat = _categoryRepo.GetById(id);
            if (cat == null) return NotFound();
            return View(cat);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Update(category);
                TempData["Success"] = "Category updated!";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public IActionResult Details(int id)
        {
            var cat = _categoryRepo.GetByIdWithMovies(id);
            if (cat == null) return NotFound();
            return View(cat);
        }

        public IActionResult Delete(int id)
        {
            var cat = _categoryRepo.GetById(id);
            if (cat == null) return NotFound();
            return View(cat);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _categoryRepo.Delete(id);
            TempData["Success"] = "Category deleted!";
            return RedirectToAction(nameof(Index));
        }
    }

    [Area("Admin")]
    public class CinemaController : Controller
    {
        private readonly ICinemaRepository _cinemaRepo;

        public CinemaController(ICinemaRepository cinemaRepo) => _cinemaRepo = cinemaRepo;

        public IActionResult Index() => View(_cinemaRepo.GetAll());
        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Cinema cinema)
        {
            if (ModelState.IsValid)
            {
                _cinemaRepo.Add(cinema);
                TempData["Success"] = "Cinema added!";
                return RedirectToAction(nameof(Index));
            }
            return View(cinema);
        }

        public IActionResult Edit(int id)
        {
            var cinema = _cinemaRepo.GetById(id);
            if (cinema == null) return NotFound();
            return View(cinema);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Cinema cinema)
        {
            if (ModelState.IsValid)
            {
                _cinemaRepo.Update(cinema);
                TempData["Success"] = "Cinema updated!";
                return RedirectToAction(nameof(Index));
            }
            return View(cinema);
        }

        public IActionResult Details(int id)
        {
            var cinema = _cinemaRepo.GetByIdWithMovies(id);
            if (cinema == null) return NotFound();
            return View(cinema);
        }

        public IActionResult Delete(int id)
        {
            var cinema = _cinemaRepo.GetById(id);
            if (cinema == null) return NotFound();
            return View(cinema);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _cinemaRepo.Delete(id);
            TempData["Success"] = "Cinema deleted!";
            return RedirectToAction(nameof(Index));
        }
    }

    [Area("Admin")]
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepo;

        public BookingController(IBookingRepository bookingRepo) => _bookingRepo = bookingRepo;

        public IActionResult Index() => View(_bookingRepo.GetAll());

        public IActionResult Details(int id)
        {
            var booking = _bookingRepo.GetById(id);
            if (booking == null) return NotFound();
            return View(booking);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, BookingStatus status)
        {
            _bookingRepo.UpdateStatus(id, status);
            TempData["Success"] = "Booking status updated!";
            return RedirectToAction(nameof(Index));
        }
    }

    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IMovieRepository _movieRepo;
        private readonly IBookingRepository _bookingRepo;
        private readonly IActorRepository _actorRepo;
        private readonly ICinemaRepository _cinemaRepo;

        public DashboardController(
            IMovieRepository movieRepo,
            IBookingRepository bookingRepo,
            IActorRepository actorRepo,
            ICinemaRepository cinemaRepo)
        {
            _movieRepo = movieRepo;
            _bookingRepo = bookingRepo;
            _actorRepo = actorRepo;
            _cinemaRepo = cinemaRepo;
        }

        public IActionResult Index()
        {
            ViewBag.TotalMovies = _movieRepo.GetAll().Count();
            ViewBag.TotalBookings = _bookingRepo.GetAll().Count();
            ViewBag.TotalActors = _actorRepo.GetAll().Count();
            ViewBag.TotalCinemas = _cinemaRepo.GetAll().Count();
            ViewBag.RecentBookings = _bookingRepo.GetAll().Take(5).ToList();
            return View();
        }
    }
}
