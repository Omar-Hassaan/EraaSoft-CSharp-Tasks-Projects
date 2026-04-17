namespace ToDoListApplication.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ToDoList> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.; Initial Catalog= ToDoListDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True;");
        }
    }
}
