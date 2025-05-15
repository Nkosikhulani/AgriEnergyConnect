using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AgriEnergyConnect.Models;

namespace AgriEnergyConnect.Data // Updated namespace
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ForumPost> ForumPosts { get; set; }
        public DbSet<TrainingCourse> TrainingCourses { get; set; }
        public DbSet<Farmer> Farmers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.User)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ForumPost>()
                .HasOne(fp => fp.User)
                .WithMany(u => u.ForumPosts)
                .HasForeignKey(fp => fp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ForumPost>()
                .Property(fp => fp.Title)
                .IsRequired();

            modelBuilder.Entity<ForumPost>()
                .Property(fp => fp.Content)
                .IsRequired();

            modelBuilder.Entity<ForumPost>()
                .Property(fp => fp.CreatedAt)
                .IsRequired();

            modelBuilder.Entity<TrainingCourse>()
                .Property(tc => tc.Title)
                .IsRequired();
            modelBuilder.Entity<TrainingCourse>()
                .Property(tc => tc.Description)
                .IsRequired();
            modelBuilder.Entity<TrainingCourse>()
                .Property(tc => tc.Instructor)
                .IsRequired();
            modelBuilder.Entity<TrainingCourse>()
                .Property(tc => tc.StartDate)
                .IsRequired();
            modelBuilder.Entity<TrainingCourse>()
                .Property(tc => tc.EndDate)
                .IsRequired();
        }
    }
}