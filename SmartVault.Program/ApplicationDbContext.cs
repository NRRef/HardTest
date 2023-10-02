using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartVault.Program.BusinessObjects;
using System.IO;

namespace SmartVault.Program
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<User> User { get; set; }
		public DbSet<Document> Document { get; set; }
		public DbSet<Account> Account { get; set; }

		private IConfigurationRoot? configuration;

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			this.SetConfiguration();
			optionsBuilder.UseSqlite($"Data Source={this.configuration["DatabaseFileName"]}");
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