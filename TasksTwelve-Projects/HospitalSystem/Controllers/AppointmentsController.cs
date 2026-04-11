namespace HospitalSystem.Controllers
{
    public class AppointmentsController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();

        public IActionResult Index()
        {
            var data = _context.Appointments
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenBy(a => a.AppointmentTime)
                .ToList();

            return View(data);
        }

        public IActionResult Create(int doctorId)
        {
            var doctor = _context.Doctors.Find(doctorId);

            if (doctor == null)
                return NotFound();

            var vm = new BookAppointmentVM
            {
                DoctorId = doctorId,
                Doctor = doctor,
                AvailableTimeSlots = GenerateSlots()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(BookAppointmentVM vm)
        {
            vm.Doctor = _context.Doctors.Find(vm.DoctorId);
            vm.AvailableTimeSlots = GenerateSlots();

            if (!ModelState.IsValid)
                return View(vm);

            var day = vm.AppointmentDate!.Value.DayOfWeek;

            if (day == DayOfWeek.Friday || day == DayOfWeek.Saturday)
            {
                ModelState.AddModelError("", "Only Sunday–Thursday allowed");
                return View(vm);
            }

            var timeSlot = TimeOnly.Parse(vm.AppointmentTime!);

            // Check for double booking
            bool alreadyBooked =  _context.Appointments.Any(a =>
                a.DoctorId == vm.DoctorId &&
                a.AppointmentDate == vm.AppointmentDate &&
                a.AppointmentTime == timeSlot);

            if (alreadyBooked)
            {
                vm.ErrorMessage = $"Dr. {vm.Doctor?.Name} already has an appointment at {timeSlot:hh:mm tt} on {vm.AppointmentDate:dd/MM/yyyy}. Please choose a different time slot.";
                return View(vm);
            }

            var appointment = new Appointment
            {
                DoctorId = vm.DoctorId,
                PatientName = vm.PatientName,
                AppointmentDate = vm.AppointmentDate.Value,
                AppointmentTime = timeSlot
            };

            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            TempData["Success"] = "Appointment booked successfully!";

            return RedirectToAction("Index");
        }

        private List<SelectListItem> GenerateSlots()
        {
            var slots = new List<SelectListItem>();

            for (int h = 9; h <= 16; h++)
            {
                slots.Add(new SelectListItem
                {
                    Value = new TimeOnly(h, 0).ToString("HH:mm"),
                    Text = new TimeOnly(h, 0).ToString("hh:mm tt")
                });

                slots.Add(new SelectListItem
                {
                    Value = new TimeOnly(h, 30).ToString("HH:mm"),
                    Text = new TimeOnly(h, 30).ToString("hh:mm tt")
                });
            }

            return slots;
        }
    }
}