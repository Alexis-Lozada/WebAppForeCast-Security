using Microsoft.EntityFrameworkCore;
using WebAppForeCast.Models;

namespace WebAppForeCast.Data
{
	public class ApplicationDBContext : DbContext
	{
		public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
		{
		}

		public DbSet<WeatherForecast> WeatherForecasts { get; set; }
		public DbSet<User> Users { get; set; } = null!;
		public DbSet<LogEntry> LogEntries { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasIndex(u => u.Username)
				.IsUnique();
			modelBuilder.Entity<User>()
				.HasIndex(u => u.Email)
				.IsUnique();

			modelBuilder.Entity<LogEntry>()
				.HasIndex(l => l.Timestamp);
		}
	}
}
