namespace HospitalSystem.Controllers
{
    public class DoctorsController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        public IActionResult Index(string name, string specialization, int page = 1)
        {
            int pageSize = 3;

            var query = _context.Doctors.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(d => d.Name.Contains(name));

            if (!string.IsNullOrEmpty(specialization))
                query = query.Where(d => d.Specialization.Contains(specialization));

            int total = query.Count();

            var doctors = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var vm = new DoctorsFilterVM
            {
                Doctors = doctors,
                SearchName = name,
                SearchSpecialization = specialization,
                CurrentPage = page,
                TotalCount = total,
                Specializations = _context.Doctors
                    .Select(d => d.Specialization)
                    .Distinct()
                    .ToList()
            };

            return View(vm);
        }
    }
}