using System.IO;
using System.Security.Principal;
using LicenseRecords.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LicenseRecords.Controllers
{
  

        public class AccountController : Controller
        {
            private readonly string _accountFilePath = "wwwroot/data/Accounts.json";
            private readonly string _productFilePath = "wwwroot/data/Products.json";

           
            public IActionResult Index()
            {
                var accounts = GetAccounts();
                return View(accounts);
            }

            private List<Accounts> GetAccounts()
            {
                
                var jsonData = System.IO.File.ReadAllText(_accountFilePath);
                var accounts = JsonConvert.DeserializeObject<List<Accounts>>(jsonData);
                return accounts;
            }
        }
    
}
