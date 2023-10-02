using System.IO.Abstractions;

namespace SmartVault.Program
{
	public partial class Program
	{
		public static IDocumentRepository documentRepository = new DocumentRepository();
		public static IFileSystem fileSystem = new FileSystem();
		public static IFileService fileService = new FileService(documentRepository, fileSystem);
		public static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				return;
			}

			fileService.WriteEveryThirdFileToFile(args[0]);
			fileService.GetAllFileSizes();
		}
	}
}