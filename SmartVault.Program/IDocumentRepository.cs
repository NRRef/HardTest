using SmartVault.Program.BusinessObjects;
using System.Collections.Generic;

namespace SmartVault.Program
{
	public interface IDocumentRepository
	{
		public List<string> GetAllFilePaths();
		public Document GetDocumentByAccountId(string accountId);
	}
}
