using Moq;
using SmartVault.Program;
using System;
using Xunit;

namespace SmartVault.Tests
{
	public class ProgramTests
	{
		[Fact]
		public void Program_WhenReceiveArgument_ShouldRunFileServiceMethods()
		{
			var fileServiceMock = new Mock<IFileService>();
			Program.Program.fileService = fileServiceMock.Object;

			string[] args = { "1" };
			Program.Program.Main(args);
			fileServiceMock.Verify(m => m.GetAllFileSizes(), Times.Once);
			fileServiceMock.Verify(m => m.WriteEveryThirdFileToFile("1"), Times.Once);
		}

		[Fact]
		public void Program_WhenNotReceiveArgument_ShouldDoNothing()
		{
			var fileServiceMock = new Mock<IFileService>();
			Program.Program.fileService = fileServiceMock.Object;

			string[] args = { "1" };
			Program.Program.Main(args);
			fileServiceMock.Verify(m => m.GetAllFileSizes(), Times.Once);
			fileServiceMock.Verify(m => m.WriteEveryThirdFileToFile("1"), Times.Once);
		}
	}
}
