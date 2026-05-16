using Microsoft.AspNetCore.Identity;

namespace MovieBookingApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string? Address { get; set; }

    }
}
