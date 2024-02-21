using Microsoft.EntityFrameworkCore;
using TheBlogg.Models;

namespace TheBlogg.Data
{
    public class TheBlogDbContext : DbContext
    {
        public TheBlogDbContext(DbContextOptions<TheBlogDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserVote> UserVotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<Article>()
                .HasOne(a => a.User)
                .WithMany(u => u.Articles)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Article>()
                .HasMany(a => a.UserVotes)
                .WithOne(uv => uv.Article)
                .HasForeignKey(uv => uv.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Article)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserVotes)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<UserVote>()
                .HasOne(uv => uv.User)
                .WithMany(u => u.UserVotes)
                .HasForeignKey(uv => uv.UserId);

            modelBuilder.Entity<UserVote>()
                .HasOne(uv => uv.Article)
                .WithMany(a => a.UserVotes)
                .HasForeignKey(uv => uv.ArticleId);
        }
    }
}
