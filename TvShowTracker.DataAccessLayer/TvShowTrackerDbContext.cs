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

        public TvShowTrackerDbContext(DbContextOptions<TvShowTrackerDbContext> options) : base(options)
        { }
    }
}
