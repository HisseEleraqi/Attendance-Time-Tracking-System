using AttendenceSystem.Models;
using Microsoft.EntityFrameworkCore;

 namespace AttendenceSystem.Data
{
    public class DataContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Employee>Employees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Intake> Intakes { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Programs> Programs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Server=.;database=Attendence;integrated security=true;trustservercertificate=true");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(user =>
            user.UseTptMappingStrategy()
            );
        }
    }
}
