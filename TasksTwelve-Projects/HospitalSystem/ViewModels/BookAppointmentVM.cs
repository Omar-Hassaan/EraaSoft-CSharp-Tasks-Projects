using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HospitalSystem.ViewModels
{
    public class BookAppointmentVM
    {
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        [Required(ErrorMessage = "Patient name is required")]
        [MaxLength(100)]
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a date")]
        [Display(Name = "Appointment Date")]
        public DateOnly? AppointmentDate { get; set; }

        [Required(ErrorMessage = "Please select a time slot")]
        [Display(Name = "Time Slot")]
        public string? AppointmentTime { get; set; }

        public List<SelectListItem> AvailableTimeSlots { get; set; } = new();

        public string? ErrorMessage { get; set; }
    }
}
