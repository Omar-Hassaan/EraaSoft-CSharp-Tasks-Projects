using Microsoft.EntityFrameworkCore;
using MovieBookingApp.Models;

namespace MovieBookingApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Explicit Many-to-Many join
            modelBuilder.Entity<MovieActor>()
                .HasKey(ma => new { ma.MovieId, ma.ActorId });

            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Movie)
                .WithMany(m => m.MovieActors)
                .HasForeignKey(ma => ma.MovieId);

            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Actor)
                .WithMany(a => a.MovieActors)
                .HasForeignKey(ma => ma.ActorId);

            // Seed data
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action" },
                new Category { Id = 2, Name = "Drama" },
                new Category { Id = 3, Name = "Comedy" },
                new Category { Id = 4, Name = "Horror" },
                new Category { Id = 5, Name = "Sci-Fi" }
            );

            modelBuilder.Entity<Cinema>().HasData(
                new Cinema { Id = 1, Name = "Cairo Cinema", Location = "Cairo, Egypt", ImageUrl = "https://images.unsplash.com/photo-1489599849927-2ee91cede3ba?w=400" },
                new Cinema { Id = 2, Name = "Alex Grand Cinema", Location = "Alexandria, Egypt", ImageUrl = "https://images.unsplash.com/photo-1517604931442-7e0c8ed2963c?w=400" },
                new Cinema { Id = 3, Name = "Giza Megaplex", Location = "Giza, Egypt", ImageUrl = "https://images.unsplash.com/photo-1560109947-543149eceb16?w=400" }
            );
        }
    }
}
