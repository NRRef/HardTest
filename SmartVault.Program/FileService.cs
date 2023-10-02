using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace SmartVault.Program
{
	public class FileService : IFileService
	{
		private readonly IDocumentRepository documentRepository;
		private readonly IFileSystem _fileSystem;

		public FileService(IDocumentRepository documentRepository, IFileSystem _fileSystem)
		{
			this.documentRepository = documentRepository;
			this._fileSystem = _fileSystem;
		}

		public void GetAllFileSizes()
		{
			var documents = documentRepository.GetAllFilePaths();
			var sumOfFileLenghts = documents.Select(m => GetFileSize(m)).Sum();
			Console.WriteLine($"Sum of files lenght: {sumOfFileLenghts}");
		}

		public void WriteEveryThirdFileToFile(string accountId)
		{
			var document = documentRepository.GetDocumentByAccountId(accountId);
			AppendTextToSingleFile(document.FilePath);
		}

		private IFileInfo GetSingleFile()
		{
			var fileName = "single-file.txt";
			var fileInfo = _fileSystem.FileInfo.New(fileName);
			if (fileInfo.Exists)
			{
				return fileInfo;
			}
			_fileSystem.FileStream.New(fileName,FileMode.Create);
			fileInfo = _fileSystem.FileInfo.New(fileName);
			return fileInfo;
		}

		private void AppendTextToSingleFile(string filePath)
		{
			var singleFile = GetSingleFile();

			var text = _fileSystem.File.ReadAllText(filePath);
			if (text.Contains("Smith Property"))
			{
				var streamWriter = singleFile.AppendText();
				streamWriter.Write(text);
				streamWriter.Close();
			}
		}

		private long GetFileSize(string filePath)
			=> _fileSystem.FileInfo.New(filePath).Length;
	}
}
