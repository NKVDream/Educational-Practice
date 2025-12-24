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

        public DbSet<Article> articles { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<Category> categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Указываем точные имена таблиц и столбцов
            modelBuilder.Entity<Article>(entity =>
            {
                entity.ToTable("articles");
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.title).HasColumnName("title");
                entity.Property(e => e.content).HasColumnName("content");
                entity.Property(e => e.excerpt).HasColumnName("excerpt");
                entity.Property(e => e.cover_image_url).HasColumnName("cover_image_url");
                entity.Property(e => e.published).HasColumnName("published");
                entity.Property(e => e.category_id).HasColumnName("category_id");
                entity.Property(e => e.author_id).HasColumnName("author_id");

                // Внешние ключи как в таблице
                entity.HasOne(a => a.category)
                    .WithMany()
                    .HasForeignKey(a => a.category_id)
                    .HasConstraintName("articles_category_id_fkey");

                entity.HasOne(a => a.author)
                    .WithMany()
                    .HasForeignKey(a => a.author_id)
                    .HasConstraintName("articles_author_id_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.username).HasColumnName("username");
                entity.Property(e => e.email).HasColumnName("email");
                entity.Property(e => e.password_hash).HasColumnName("password_hash"); // ← ДОБАВЛЕНО
                entity.Property(e => e.user_role).HasColumnName("user_role");

                entity.HasOne(u => u.role)
                    .WithMany()
                    .HasForeignKey(u => u.user_role)
                    .HasConstraintName("users_user_role_fkey");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.name).HasColumnName("name");
                entity.Property(e => e.description).HasColumnName("description");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.name).HasColumnName("name");
                entity.Property(e => e.description).HasColumnName("description");
            });
        }
    }
}
