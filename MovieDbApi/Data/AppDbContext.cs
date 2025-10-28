using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieApi.Models;
using MovieDbApi.Models;

namespace MovieDbApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Movie> Movies => Set<Movie>();

        public DbSet<Favourite> Favourites { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // CreatedOn default value
            modelBuilder.Entity<Movie>()
                .Property(p => p.CreatedOn)
                .HasDefaultValueSql("GETUTCDATE()");

            // ReleaseDate conversion
            modelBuilder.Entity<Movie>()
                .Property(m => m.ReleaseDate)
                .HasConversion(
                    v => v.HasValue ? v.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                    v => v.HasValue ? DateOnly.FromDateTime(v.Value) : (DateOnly?)null
                );

            //UserName Is Email and unique
            //modelBuilder.Entity<UserDetail>().HasIndex(u => u.Username).IsUnique();
        }
    }
}