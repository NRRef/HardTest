namespace SmartVault.DataGeneration.BusinessObjects
{
    public partial class User
    {
        public string FullName => $"{FirstName} {LastName}";
    }
}
