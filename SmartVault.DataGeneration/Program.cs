using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace SmartVault.DataGeneration
{
	partial class Program
	{
		static void Main(string[] args)
		{
			using (var context = new ApplicationDbContext())
			{
				context.Database.EnsureCreated();
				context.Database.Migrate();
				var accountData = context.Account.Count();
				Console.WriteLine($"AccountCount: {JsonConvert.SerializeObject(accountData)}");
				var documentData = context.Document.Count();
				Console.WriteLine($"DocumentCount: {JsonConvert.SerializeObject(documentData)}");
				var userData = context.User.First();
				Console.WriteLine($"UserCount: {JsonConvert.SerializeObject(userData)}");
			}
		}
	}
}