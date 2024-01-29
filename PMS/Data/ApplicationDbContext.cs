using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PMS.Models;

namespace PMS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        public DbSet<PMS.Models.Employee> Employees { get; set; }
        public DbSet<PMS.Models.Project> Project { get; set; } 
        public DbSet<PMS.Models.Task> Task { get; set; }
        public DbSet<PMS.Models.Comment> Comment { get; set; }
        public DbSet<PMS.Models.Status> Status { get; set; }
        public DbSet<PMS.Models.Priority> Priority { get; set; }

        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Test>().ToTable("Test");
            modelBuilder.Entity<Mazin>().ToTable("Mazin");
        }
        */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Disable cascade delete for Project-Tasks relationship
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);  // Adjust Cascade or SetNull based on your requirements

            // Disable cascade delete for Task-Status relationship
            modelBuilder.Entity<Models.Task>()
                .HasOne(t => t.Status)
                .WithMany(s => s.Task)
                .HasForeignKey(t => t.StatusId)
                .OnDelete(DeleteBehavior.SetNull);

            // Disable cascade delete for Status-Tasks relationship
            modelBuilder.Entity<Status>()
                .HasMany(s => s.Task)
                .WithOne(t => t.Status)
                .HasForeignKey(t => t.StatusId)
                .OnDelete(DeleteBehavior.SetNull);

            // Disable cascade delete for Project-Status relationship
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Status)
                .WithMany()
                .HasForeignKey(p => p.StatusId)
                .OnDelete(DeleteBehavior.SetNull);

            // Add other configurations as needed

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<PMS.Models.UserModel> UserModel { get; set; } = default!;

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Disable cascade delete for both relationships
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Models.Task>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Status>()
                  .HasMany(p => p.Project)

            // Other configurations...




            base.OnModelCreating(modelBuilder);
        }*/


    }
}