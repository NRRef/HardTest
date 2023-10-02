using Microsoft.EntityFrameworkCore;
using SmartVault.DataGeneration.BusinessObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class DataBaseSeed
{
	const int ACCOUNT_LENGTH = 100;
	const int DOCUMENT_LENGTH = 10000;
	const int TEXT_REPEAT_TIMES = 101;
	const string TEST_FILENAME = "TestDoc.txt";
	const string TEST_FILE_TEXT = "This is my test document\n";
	

	public static void Seed(this ModelBuilder modelBuilder)
	{
		CreateFile();
		var users = GetUserSeedList();
		var accounts = GetAccountSeedList();
		var documents = GetDocumentsSeedList(accounts);

		modelBuilder.Entity<User>().HasData(users);
		modelBuilder.Entity<Account>().HasData(accounts);
		modelBuilder.Entity<Document>().HasData(documents);
	}

	private static void CreateFile()
	{
		var text = Enumerable.Range(0, TEXT_REPEAT_TIMES)
			.Select(_ => TEST_FILE_TEXT)
			.Aggregate((current, next) => current + next);

		File.WriteAllText(TEST_FILENAME, text);
	}
	private static List<User> GetUserSeedList()
	{
		var users = new List<User>();
		for (var i = 1; i <= ACCOUNT_LENGTH; i++)
		{
			var randomDayIterator = RandomDay().GetEnumerator();
			randomDayIterator.MoveNext();
			users.Add(new User()
			{
				Id = i,
				FirstName = $"FName{i}",
				LastName = $"LName{i}",
				DateOfBirth = randomDayIterator.Current,
				AccountId = i,
				Username = $"UserName-{i}",
				Password = "e10adc3949ba59abbe56e057f20f883e"
			});
		}
		return users;
	}

	private static List<Account> GetAccountSeedList()
	{
		var accounts = new List<Account>();
		for (var i = 1; i <= ACCOUNT_LENGTH; i++)
		{
			accounts.Add(new Account()
			{
				Id = i,
				Name = $"Account{i}"
			});
		}
		return accounts;
	}

	private static List<Document> GetDocumentsSeedList(List<Account> accounts)
	{
		var documents = new List<Document>();
		var documentNumber = 1;
		var documentPath = new FileInfo(TEST_FILENAME).FullName;
		var documentLength = new FileInfo(documentPath).Length;
		accounts.ForEach(item =>
		{
			for (var i = 1; i <= DOCUMENT_LENGTH; i++)
			{
				documents.Add(new Document()
				{
					Id = documentNumber,
					Name = $"Document{item.Id}-{i}.txt",
					FilePath = documentPath,
					Length = ((int)documentLength),
					AccountId = item.Id

				});
				documentNumber++;
			}
		});

		return documents;
	}

	private static IEnumerable<DateTime> RandomDay()
	{
		DateTime start = new DateTime(1985, 1, 1);
		Random gen = new Random();
		int range = (DateTime.Today - start).Days;
		while (true)
			yield return start.AddDays(gen.Next(range));
	}
}