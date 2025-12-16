// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using NewsApp.Models;

namespace NewsApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Таблицы
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка таблицы Users
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();

                entity.HasOne(u => u.UserRole)
                      .WithMany(r => r.Users)
                      .HasForeignKey(u => u.UserRoleId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Настройка таблицы Articles
            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasOne(a => a.Category)
                      .WithMany(c => c.Articles)
                      .HasForeignKey(a => a.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Author)
                      .WithMany(u => u.Articles)
                      .HasForeignKey(a => a.AuthorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Настройка таблицы Roles
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(r => r.Name).IsUnique();
            });

            // Настройка таблицы Categories
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(c => c.Name).IsUnique();
            });
        }
    }
}
