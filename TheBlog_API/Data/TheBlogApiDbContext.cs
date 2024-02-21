using Microsoft.EntityFrameworkCore;
using TheBlog_API.Models;

namespace TheBlog_API.Data
{
    public partial class TheBlogApiDbContext : DbContext
    {
        public TheBlogApiDbContext()
        {
        }

        public TheBlogApiDbContext(DbContextOptions<TheBlogApiDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Article> Articles { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LVAGPLTP5487\\SQLEXPRESS01;Database=master;Integrated Security=True;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_Articles_UserId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasIndex(e => e.ArticleId, "IX_Comments_ArticleId");

                entity.HasIndex(e => e.UserId, "IX_Comments_UserId");

                entity.Property(e => e.IsComplained)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.ArticleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "IX_Users_Email")
                    .IsUnique()
                    .HasFilter("([Email] IS NOT NULL)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
