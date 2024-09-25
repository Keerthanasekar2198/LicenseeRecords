using LicenseRecords.Models;
using static LicenseRecords.Models.Accounts;

namespace LicenseRecords.Services
{
 
    public interface IAccountService
    {
        IEnumerable<Accounts> GetAllAccounts();
        Accounts GetAccountById(int id);
        void CreateAccount(Accounts account);
        void UpdateAccount(Accounts account);
        void DeleteAccount(int id);
        public Task<IEnumerable<Product>> GetProductNamesAsync();
    }

}
