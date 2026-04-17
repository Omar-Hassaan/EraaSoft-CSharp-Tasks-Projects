namespace ToDoListApplication.Controllers
{
    public class TaskController : Controller
    {
        ApplicationDbContext _Context = new ApplicationDbContext();
        public IActionResult Index()
        {
            var tasks = _Context.Tasks.ToList();
            var user = _Context.Users.OrderBy(e => e.Id).LastOrDefault();
            IndexVM vm = new IndexVM() { Tasks = tasks, User = user };

            return View(vm);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ToDoList task, IFormFile file11)
        {
            if (file11 != null)
            {                   // dshda7ta-asdahakjd.png
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file11.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\assets", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    file11.CopyTo(stream);
                }

                task.File = fileName; // dshda7ta-asdahakjd.png
            }
            _Context.Tasks.Add(task);
            _Context.SaveChanges();
            return RedirectToAction("Index");

        }

        public IActionResult GetName()
        {
            return View();
        }

        public IActionResult GetName(string Name)
        {
            Models.User u = new Models.User() { Name = Name };
            _Context.Users.Add(u);
            _Context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
