using System.ComponentModel.DataAnnotations;

namespace HospitalSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Patient name is required")]
        [MaxLength(100)]
        [Display(Name = "Patient Name")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "Please select a date")]
        [Display(Name = "Appointment Date")]
        public DateOnly AppointmentDate { get; set; }

        [Required(ErrorMessage = "Please select a time slot")]
        [Display(Name = "Appointment Time")]
        public TimeOnly AppointmentTime { get; set; }

        [Required]
        public int DoctorId { get; set; }

        public Doctor? Doctor { get; set; }
    }
}
