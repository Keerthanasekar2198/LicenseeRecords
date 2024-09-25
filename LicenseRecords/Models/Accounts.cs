namespace LicenseRecords.Models
{
    public class Accounts
    {
        public int AccountId { get; set; }
        public string? AccountName { get; set; }
        public string? AccountStatus { get; set; }
        public List<ProductLicenseDetail>? ProductLicence { get; set; }

       
        public class ProductLicenseDetail
        {
            public int LicenceId { get; set; }
            public string? LicenceStatus { get; set; }
            public DateTime LicenceFromDate { get; set; }
            public DateTime? LicenceToDate { get; set; }  
            public Product? Product { get; set; }
        }

        
        public class Product
        {
            public int ProductId { get; set; }
            public string? ProductName { get; set; }
        }
    }
}
