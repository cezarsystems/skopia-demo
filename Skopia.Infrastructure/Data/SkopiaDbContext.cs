using Microsoft.EntityFrameworkCore;
using Skopia.Domain.Models;

namespace Skopia.Infrastructure.Data
{
    public class SkopiaDbContext : DbContext
    {
        public SkopiaDbContext(DbContextOptions<SkopiaDbContext> options)
            : base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<TaskHistoryModel> TaskHistories { get; set; }
        public DbSet<TaskCommentModel> TaskComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserModel>().ToTable("Users");
            modelBuilder.Entity<ProjectModel>().ToTable("Projects");
            modelBuilder.Entity<TaskModel>().ToTable("Tasks");
            modelBuilder.Entity<TaskHistoryModel>().ToTable("TaskHistories");
            modelBuilder.Entity<TaskCommentModel>().ToTable("TaskComments");

            modelBuilder.Entity<UserModel>().HasData(
                new UserModel { Id = 1, Name = "Administrador do JIRA", Role = "adm" },
                new UserModel { Id = 2, Name = "Project Manager", Role = "mgr" },
                new UserModel { Id = 3, Name = "Agile Master", Role = "am" },
                new UserModel { Id = 4, Name = "Product Owner", Role = "po" },
                new UserModel { Id = 5, Name = "Common User", Role = "usr" }
            );
        }
    }
}