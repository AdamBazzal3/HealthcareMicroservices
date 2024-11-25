namespace HealthcareBillAPI.Settings.DbSettings
{
    public class HealthcareDbSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string InvoicesCollectionName { get; set; } = null!;
        public string ProductsCollectionName { get; set; } = null!;
        public string EmployeeCollectionName { get; set; } = null!;

    }
}
