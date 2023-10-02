namespace SmartVault.Program
{
	public interface IFileService {
		public void GetAllFileSizes();
		public void WriteEveryThirdFileToFile(string accountId);
	}
}
