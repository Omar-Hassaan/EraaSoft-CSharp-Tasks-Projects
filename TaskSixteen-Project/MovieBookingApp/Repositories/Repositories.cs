using Microsoft.EntityFrameworkCore;
using MovieBookingApp.Data;
using MovieBookingApp.Interfaces;
using MovieBookingApp.Models;

namespace MovieBookingApp.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Movie> GetAll(int? categoryId = null, int? cinemaId = null)
        {
            var query = _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(m => m.CategoryId == categoryId.Value);

            if (cinemaId.HasValue)
                query = query.Where(m => m.CinemaId == cinemaId.Value);

            return query.ToList();
        }

        public Movie? GetById(int id) =>
            _context.Movies.Find(id);

        public Movie? GetByIdWithDetails(int id) =>
            _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .FirstOrDefault(m => m.Id == id);

        public IEnumerable<Movie> Search(string term) =>
            _context.Movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .Where(m => m.Name.Contains(term))
                .ToList();

        public void Add(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
        }

        public void Update(Movie movie)
        {
            _context.Movies.Update(movie);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
        }
    }

    public class ActorRepository : IActorRepository
    {
        private readonly ApplicationDbContext _context;

        public ActorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Actor> GetAll() => _context.Actors.ToList();

        public Actor? GetById(int id) => _context.Actors.Find(id);

        public Actor? GetByIdWithMovies(int id) =>
            _context.Actors
                .Include(a => a.MovieActors)
                    .ThenInclude(ma => ma.Movie)
                .FirstOrDefault(a => a.Id == id);

        public void Add(Actor actor)
        {
            _context.Actors.Add(actor);
            _context.SaveChanges();
        }

        public void Update(Actor actor)
        {
            _context.Actors.Update(actor);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var actor = _context.Actors.Find(id);
            if (actor != null)
            {
                _context.Actors.Remove(actor);
                _context.SaveChanges();
            }
        }
    }

    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAll() => _context.Categories.ToList();

        public Category? GetById(int id) => _context.Categories.Find(id);

        public Category? GetByIdWithMovies(int id) =>
            _context.Categories
                .Include(c => c.Movies)
                .FirstOrDefault(c => c.Id == id);

        public void Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }
    }

    public class CinemaRepository : ICinemaRepository
    {
        private readonly ApplicationDbContext _context;

        public CinemaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Cinema> GetAll() => _context.Cinemas.ToList();

        public Cinema? GetById(int id) => _context.Cinemas.Find(id);

        public Cinema? GetByIdWithMovies(int id) =>
            _context.Cinemas
                .Include(c => c.Movies)
                .FirstOrDefault(c => c.Id == id);

        public void Add(Cinema cinema)
        {
            _context.Cinemas.Add(cinema);
            _context.SaveChanges();
        }

        public void Update(Cinema cinema)
        {
            _context.Cinemas.Update(cinema);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var cinema = _context.Cinemas.Find(id);
            if (cinema != null)
            {
                _context.Cinemas.Remove(cinema);
                _context.SaveChanges();
            }
        }
    }

    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Booking> GetAll() =>
            _context.Bookings.Include(b => b.Movie).OrderByDescending(b => b.BookingDate).ToList();

        public Booking? GetById(int id) =>
            _context.Bookings.Include(b => b.Movie).FirstOrDefault(b => b.Id == id);

        public IEnumerable<Booking> GetByEmail(string email) =>
            _context.Bookings
                .Include(b => b.Movie)
                    .ThenInclude(m => m!.Cinema)
                .Where(b => b.CustomerEmail == email)
                .OrderByDescending(b => b.BookingDate)
                .ToList();

        public void Add(Booking booking)
        {
            _context.Bookings.Add(booking);
            _context.SaveChanges();
        }

        public void UpdateStatus(int id, BookingStatus status)
        {
            var booking = _context.Bookings.Find(id);
            if (booking != null)
            {
                booking.Status = status;
                _context.SaveChanges();
            }
        }
    }
}
