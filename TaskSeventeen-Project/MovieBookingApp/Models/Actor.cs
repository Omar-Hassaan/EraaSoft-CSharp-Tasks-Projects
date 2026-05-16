using System.ComponentModel.DataAnnotations;

namespace MovieBookingApp.Models
{
    public class Actor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Actor name is required")]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [StringLength(1000)]
        public string? Bio { get; set; }

        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
    }
}
