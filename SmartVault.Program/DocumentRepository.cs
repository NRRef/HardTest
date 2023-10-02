using SmartVault.Program.BusinessObjects;
using System.Collections.Generic;
using System.Linq;

namespace SmartVault.Program
{
	public class DocumentRepository : IDocumentRepository
	{
		public List<string> GetAllFilePaths()
		{
			List<string> documents;
			using (var context = new ApplicationDbContext())
			{
				documents = context.Document.Select(m => m.FilePath).ToList();
			}
			return documents;
		}

		public Document GetDocumentByAccountId(string accountId)
		{
			Document document;
			using (var context = new ApplicationDbContext())
			{
				document = context.Document.OrderBy(m => m.AccountId).Skip(2).FirstOrDefault();
			}
			return document;
		}
	}
}
