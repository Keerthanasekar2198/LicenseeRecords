using Microsoft.AspNetCore.Mvc;
using LicenseRecords.Models;
using LicenseRecords.Services;
using System.Collections.Generic;
using static LicenseRecords.Models.Accounts;

[Route("api/AccountsApi")]
[ApiController]
public class AccountsApiController : ControllerBase
{
    private readonly IAccountService _accountsService;

    public AccountsApiController(IAccountService accountsService)
    {
        _accountsService = accountsService;
    }



    // GET: api/accounts
    [HttpGet]
    public ActionResult<IEnumerable<Accounts>> GetAccounts()
    {
        var accounts = _accountsService.GetAllAccounts();
        return Ok(accounts);
    }



    // GET: api/accounts/{id}
    [HttpGet("{id}")]
    public ActionResult<Accounts> GetAccount(int id)
    {
        var account = _accountsService.GetAccountById(id);
        if (account == null)
        {
            return NotFound();
        }
        return Ok(account);
    }



    // POST: api/accounts
    [HttpPost]
    public ActionResult<Accounts> CreateAccount(Accounts account)
    {
        if (account == null)
        {
            return BadRequest();
        }

        _accountsService.CreateAccount(account);
        return CreatedAtAction(nameof(GetAccount), new { id = account.AccountId }, account);
    }



    // PUT: api/AccountsApi/UpdateAccount/{id}
    [HttpPut("{id}")]
    public ActionResult UpdateAccount(int id, [FromBody] Accounts account)
    {
      
        if (account == null)
        {
            return BadRequest("Account data cannot be null.");
        }

        
        var existingAccount = _accountsService.GetAccountById(id);
        if (existingAccount == null)
        {
            return NotFound();
        }

        
        existingAccount.AccountName = account.AccountName;
        existingAccount.AccountStatus = account.AccountStatus;

        try
        {
           
            _accountsService.UpdateAccount(existingAccount);
            return NoContent(); 
        }
        catch (Exception ex)
        {
            
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }



    // DELETE: api/accounts/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteAccount(int id)
    {
        var account = _accountsService.GetAccountById(id);
        if (account == null)
        {
            return NotFound();
        }

        _accountsService.DeleteAccount(id);
        return NoContent();
    }

    [HttpGet("products")]
    public async Task<ActionResult<IEnumerable<string>>> GetProducts()
    {
        var productNames = await _accountsService.GetProductNamesAsync();
        return Ok(productNames);
    }


    // DELETE: api/AccountsApi/DeleteLicense/{licenceId}
    [HttpDelete("DeleteLicense/{licenceId}")]
    public ActionResult DeleteLicense(int licenceId)
    {
        var licenseDeleted = _accountsService.DeleteLicense(licenceId);

        if (!licenseDeleted)
        {
            return NotFound(); 
        }

        return NoContent();
    }


    [HttpPost("AddLicense/{accountId}")]
    public IActionResult AddLicense(int accountId, [FromBody] Accounts.ProductLicenseDetail newLicense)
    {
        var account = _accountsService.GetAccountById(accountId);

        if (account == null)
        {
            return NotFound();
        }


        var products = _accountsService.GetAllProducts();


        var product = products.FirstOrDefault(p => p.ProductId == newLicense.ProductId);


        if (product == null)
        {
            return BadRequest("Invalid product ID.");
        }

       
        newLicense.Product = product;

        if (account.ProductLicence == null)
        {
            account.ProductLicence = new List<Accounts.ProductLicenseDetail>();
        }

       
        newLicense.LicenceId = account.ProductLicence.Count > 0 ? account.ProductLicence.Max(l => l.LicenceId) + 1 : 1;

        
        account.ProductLicence.Add(newLicense);

        
        _accountsService.UpdateAccount(account);

        return Ok(newLicense);
    }


}
