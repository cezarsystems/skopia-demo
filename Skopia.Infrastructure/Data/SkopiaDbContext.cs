using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Skopia.Domain.Models;

namespace Skopia.Infrastructure.Data
{
    public class SkopiaDbContext : DbContext
    {
        public SkopiaDbContext(DbContextOptions<SkopiaDbContext> options)
            : base(options) { }

        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<TaskHistoryModel> TaskHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserModel>()
                .ToTable("Users");

            modelBuilder.Entity<UserModel>().HasData(
                new() { Id = 1, Name = "Administrador do JIRA", Role = "admin" },
                new() { Id = 2, Name = "Agile Master", Role = "manager" },
                new() { Id = 3, Name = "Product Owner", Role = "po" },
                new() { Id = 4, Name = "Common User", Role = "user" }
            );

            modelBuilder.Entity<ProjectModel>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProjectModel>()
                .HasOne(p => p.User)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskModel>()
                .Property(t => t.Comments)
                .HasConversion(
                    v => string.Join("||", v),
                    v => v.Split("||", StringSplitOptions.None)
                )
                .Metadata.SetValueComparer(new ValueComparer<string[]>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToArray()
                ));

            modelBuilder.Entity<TaskModel>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskHistoryModel>(entity =>
            {
                entity.ToTable("TaskHistories");

                entity.HasKey(h => h.Id);

                entity.Property(h => h.FieldChanged)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(h => h.OldValue)
                    .HasMaxLength(500);

                entity.Property(h => h.NewValue)
                    .HasMaxLength(500);

                entity.HasOne(h => h.Task)
                    .WithMany(t => t.Histories)
                    .HasForeignKey(h => h.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(h => h.User)
                    .WithMany(u => u.Histories)
                    .HasForeignKey(h => h.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}