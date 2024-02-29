using EmployeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee()
                {
                    Id = 1,
                    Name = "Zhiji Wang",
                    Job = "Tiktokerist",
                    Country = "China",
                    CreatedDate = DateTime.Now
                },
                new Employee()
                {
                    Id = 2,
                    Name = "Sandara Park",
                    Job = "Dancerist",
                    Country = "South Korea",
                    CreatedDate = DateTime.Now
                },
                new Employee()
                {
                    Id = 3,
                    Name = "Bert Nguyen",
                    Job = "Dancerist",
                    Country = "Vietnam",
                    CreatedDate = DateTime.Now
                }
                );
        }
    }
}
