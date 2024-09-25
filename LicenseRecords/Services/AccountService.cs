using LicenseRecords.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using static LicenseRecords.Models.Accounts;

namespace LicenseRecords.Services
{
    public class AccountService : IAccountService
    {
        private readonly string _accountsJsonPath;
        private readonly string _productsJsonPath;
        private List<Accounts> _accounts;

        public AccountService(IWebHostEnvironment webHostEnvironment)
        {
            // Construct paths relative to the wwwroot folder
            _accountsJsonPath = Path.Combine(webHostEnvironment.WebRootPath, "data", "Accounts.json");
            _productsJsonPath = Path.Combine(webHostEnvironment.WebRootPath, "data", "Products.json");

            // Load accounts from JSON on initialization
            _accounts = LoadAccountsFromJson();
        }

        public static List<T> LoadJsonData<T>(string jsonFilePath)
        {
            if (!File.Exists(jsonFilePath))
            {
                throw new FileNotFoundException($"JSON file not found at path: {jsonFilePath}");
            }

            var jsonData = File.ReadAllText(jsonFilePath);
            return JsonSerializer.Deserialize<List<T>>(jsonData) ?? new List<T>();
        }

        private List<Accounts> LoadAccountsFromJson()
        {
            // Load the accounts and their associated products from JSON
            var accounts = LoadJsonData<Accounts>(_accountsJsonPath);
            var products = LoadJsonData<Accounts.Product>(_productsJsonPath);

            // Map products to their respective accounts
            foreach (var account in accounts)
            {
                if (account.ProductLicence != null)
                {
                    foreach (var licence in account.ProductLicence)
                    {
                        licence.Product = products.FirstOrDefault(p => p.ProductId == licence.Product?.ProductId);
                    }
                }
            }
            return accounts;
        }

        public IEnumerable<Accounts> GetAllAccounts()
        {
            return _accounts;
        }

        public Accounts GetAccountById(int id)
        {
            return _accounts.FirstOrDefault(a => a.AccountId == id);
        }

        public void CreateAccount(Accounts account)
        {
            account.AccountId = _accounts.Count > 0 ? _accounts.Max(a => a.AccountId) + 1 : 1;
            account.ProductLicence = null;
            _accounts.Add(account);
            SaveAccountsToJson(); // Save the new account to JSON
        }

        public void UpdateAccount(Accounts account)
        {
            var existingAccount = GetAccountById(account.AccountId);
            if (existingAccount != null)
            {
                existingAccount.AccountName = account.AccountName;
                existingAccount.AccountStatus = account.AccountStatus;
                existingAccount.ProductLicence = account.ProductLicence;
                SaveAccountsToJson(); // Update the JSON file
            }
        }

        public void DeleteAccount(int id)
        {
            var account = GetAccountById(id);
            if (account != null)
            {
                _accounts.Remove(account);
                SaveAccountsToJson(); // Save the changes to the JSON file
            }
        }

        private void SaveAccountsToJson()
        {
            var jsonData = JsonSerializer.Serialize(_accounts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_accountsJsonPath, jsonData);
        }

        public async Task<IEnumerable<Product>> GetProductNamesAsync()
        {
            var json = await File.ReadAllTextAsync(_productsJsonPath);
            var products = JArray.Parse(json)
                .Select(p => new Product
                {
                    ProductId = (int)p["ProductId"],
                    ProductName = p["ProductName"].ToString()
                })
                .ToList();

            return products;
        }
    }
}
