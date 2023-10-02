using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartVault.DataGeneration.BusinessObjects;
using System.IO;

namespace SmartVault.DataGeneration
{
	public class ApplicationDbContext : DbContext
	{
		private readonly string DATABASE_FILENAME_PROPERTY = "DatabaseFileName";
		private readonly string DATETIME_SQLITE_FUNCTION = "strftime('%Y-%m-%d %H:%M:%S', 'now')";
		public DbSet<User> User { get; set; }
		public DbSet<Document> Document { get; set; }
		public DbSet<Account> Account { get; set; }

		private IConfigurationRoot? configuration;
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			this.SetConfiguration();
			optionsBuilder.UseSqlite($"Data Source=..\\..\\..\\{this.configuration[DATABASE_FILENAME_PROPERTY]}");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			this.SetConfiguration();
			System.Data.SQLite.SQLiteConnection.CreateFile(this.configuration[DATABASE_FILENAME_PROPERTY]);
			modelBuilder.Entity<User>().Property(p => p.CreatedAt).HasDefaultValueSql(DATETIME_SQLITE_FUNCTION);
			modelBuilder.Entity<Account>().Property(p => p.CreatedAt).HasDefaultValueSql(DATETIME_SQLITE_FUNCTION);
			modelBuilder.Entity<Document>().Property(p => p.CreatedAt).HasDefaultValueSql(DATETIME_SQLITE_FUNCTION);

			modelBuilder.Seed();
			base.OnModelCreating(modelBuilder);
		}

		private void SetConfiguration()
		{
			this.configuration = this.configuration is null
				? new ConfigurationBuilder()
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json").Build()
				: this.configuration;
		}
	}
}