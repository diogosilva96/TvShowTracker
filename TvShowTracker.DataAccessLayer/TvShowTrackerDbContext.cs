using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.DataAccessLayer.Models;

namespace TvShowTracker.DataAccessLayer
{
    public class TvShowTrackerDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<TvShow> Shows { get; set; }

        public DbSet<Actor> Actors { get; set; }

        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public DbSet<Role> Roles { get; set; }

        public TvShowTrackerDbContext(DbContextOptions<TvShowTrackerDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                        .Property(b => b.RegisteredAt)
                        .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<User>()
                        .Property(b => b.LastUpdatedAt)
                        .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<TvShow>()
                        .Property(b => b.AddedAt)
                        .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<Episode>()
                        .Property(b => b.AddedAt)
                        .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<User>()
                        .Property(b => b.IsActive)
                        .HasDefaultValueSql("1");

            modelBuilder.Entity<TvShow>()
                        .Property(b => b.IsActive)
                        .HasDefaultValueSql("1");

            modelBuilder.Entity<Episode>()
                        .Property(b => b.IsActive)
                        .HasDefaultValueSql("1"); 

            modelBuilder.Entity<Genre>()
                        .Property(b => b.IsActive)
                        .HasDefaultValueSql("1");

            modelBuilder.Entity<Actor>()
                        .Property(b => b.IsActive)
                        .HasDefaultValueSql("1");
        }
    }
}
