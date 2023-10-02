using Moq;
using SmartVault.Program;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using Xunit;

namespace SmartVault.Tests
{
	public class FileServiceTests
	{
		[Fact]
		public void GetAllFileSizes_ShouldCalculateSumOfSizes()
		{
			var documentRepositoryMock = new Mock<IDocumentRepository>();
			var fileSystemMock = new Mock<IFileSystem>();
			var fileSystemStreamMock = new Mock<IFileStreamFactory>();
			var fileInfoMock = new Mock<IFileInfo>();
			fileInfoMock.Setup(m => m.Length).Returns(1000);
			fileSystemMock.Setup(m => m.FileStream.New(It.IsAny<string>(), FileMode.Create)).Returns(fileSystemStreamMock.Object.New(It.IsAny<string>(), FileMode.Create));
			fileSystemMock.Setup(m => m.File.ReadAllText(It.IsAny<string>())).Returns("test");
			fileSystemMock.Setup(m => m.FileInfo.New(It.IsAny<string>())).Returns(fileInfoMock.Object);

			documentRepositoryMock.Setup(repo => repo.GetAllFilePaths()).Returns(new List<string>
			{
				"file1",
				"file2",
				"file3"
			});

			var fileService = new FileService(documentRepositoryMock.Object, fileSystemMock.Object);
			using (var sw = new StringWriter())
			{
				Console.SetOut(sw);
				fileService.GetAllFileSizes();
				var result = sw.ToString().Trim();
				Assert.Equal("Sum of files lenght: 3000", result);
			}

			fileSystemMock.Verify(m => m.FileInfo.New(It.IsAny<string>()), Times.Exactly(3));
		}

		[Fact]
		public void WriteEveryThirdFileToFile_WhenFindTheProperty_ShouldWriteToFile()
		{
			var documentRepositoryMock = new Mock<IDocumentRepository>();

			var fileSystemMock = new Mock<IFileSystem>();
			var fileSystemStreamMock = new Mock<IFileStreamFactory>();
			var fileInfoMock = new Mock<IFileInfo>();
			var streamWriterMock = new Mock<StreamWriter>("test");

			streamWriterMock.Setup(m => m.Write(It.IsAny<string>()));
			streamWriterMock.Setup(m => m.Close());

			fileSystemMock.Setup(m => m.FileStream.New(It.IsAny<string>(), FileMode.Create)).Returns(fileSystemStreamMock.Object.New(It.IsAny<string>(), FileMode.Create));
			fileSystemMock.Setup(m => m.File.ReadAllText(It.IsAny<string>())).Returns("teSmith Propertyst");
			fileSystemMock.Setup(m => m.FileInfo.New(It.IsAny<string>())).Returns(fileInfoMock.Object);

			fileInfoMock.Setup(m => m.AppendText()).Returns(streamWriterMock.Object);

			documentRepositoryMock.Setup(repo => repo.GetDocumentByAccountId(It.IsAny<string>())).Returns(new Program.BusinessObjects.Document() { FilePath = "testFilePath" });

			var fileService = new FileService(documentRepositoryMock.Object, fileSystemMock.Object);
			fileService.WriteEveryThirdFileToFile("id");

			streamWriterMock.Verify(m => m.Write("teSmith Propertyst"), Times.Once);
		}

		[Fact]
		public void WriteEveryThirdFileToFile_WhenNotFindTheProperty_ShouldDoNothing()
		{
			var documentRepositoryMock = new Mock<IDocumentRepository>();

			var fileSystemMock = new Mock<IFileSystem>();
			var fileSystemStreamMock = new Mock<IFileStreamFactory>();
			var fileInfoMock = new Mock<IFileInfo>();
			var streamWriterMock = new Mock<StreamWriter>("test1");

			streamWriterMock.Setup(m => m.Write(It.IsAny<string>()));
			streamWriterMock.Setup(m => m.Close());

			fileSystemMock.Setup(m => m.FileStream.New(It.IsAny<string>(), FileMode.Create)).Returns(fileSystemStreamMock.Object.New(It.IsAny<string>(), FileMode.Create));
			fileSystemMock.Setup(m => m.File.ReadAllText(It.IsAny<string>())).Returns("test");
			fileSystemMock.Setup(m => m.FileInfo.New(It.IsAny<string>())).Returns(fileInfoMock.Object);

			fileInfoMock.Setup(m => m.AppendText()).Returns(streamWriterMock.Object);

			documentRepositoryMock.Setup(repo => repo.GetDocumentByAccountId(It.IsAny<string>())).Returns(new Program.BusinessObjects.Document() { FilePath = "testFilePath" });

			var fileService = new FileService(documentRepositoryMock.Object, fileSystemMock.Object);
			fileService.WriteEveryThirdFileToFile("id");

			streamWriterMock.Verify(m => m.Write(It.IsAny<string>()), Times.Never);
		}
	}
}
