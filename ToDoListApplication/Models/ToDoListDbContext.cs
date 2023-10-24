using Microsoft.EntityFrameworkCore;

namespace ToDoListApplication.Models
{
    public class ToDoListDbContext : DbContext
    {
        public ToDoListDbContext(DbContextOptions<ToDoListDbContext> options) : base(options)
        {
        }

        public DbSet<ToDo> ToDos { get; set; } = null!;

        public DbSet<Priority> Priorities { get; set; } = null!;

        public DbSet<Status> Statuses { get; set; } = null!;


        //seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Priority>().HasData(
                new Priority { PriorityId = "verylow", PriorityName = "Very Low" },
                new Priority { PriorityId = "low", PriorityName = "Low"},
                new Priority { PriorityId = "medium", PriorityName = "Medium" },
                new Priority { PriorityId = "high", PriorityName = "High" },
                new Priority { PriorityId = "veryhigh", PriorityName = "Very High" }
            );

            modelBuilder.Entity<Status>().HasData(
                new Status { StatusId = "notstarted", StatusName = "Not Started" },
                new Status { StatusId = "inprogress", StatusName = "In Progress" },
                new Status { StatusId = "completed", StatusName = "Completed" }
            );
        }
    }
}
