﻿using Microsoft.AspNetCore.Mvc;
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
    public ActionResult<Accounts> UpdateAccount(int id, [FromBody] Accounts account)
    {
        if (id != account.AccountId) 
        {
            return BadRequest("Account ID mismatch.");
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
}
