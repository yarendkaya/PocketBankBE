using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocketBankBE.DTOs;
using PocketBankBE.Models;
using PocketBankBE.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PocketBankBE.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Bu controller'a art�k sadece giri� yapm�� kullan�c�lar eri�ebilir
public class AccountController : ControllerBase
{
    private readonly AccountService _service;

    public AccountController(AccountService service)
    {
        _service = service;
    }

    // Giri� yapm�� kullan�c�n�n ID'sini token'dan okur
    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // Ger�ek bir uygulamada TryParse daha g�venli olur
        return int.Parse(userIdClaim!);
    }

    // Sadece giri� yapm�� kullan�c�n�n hesaplar�n� getirir
    [HttpGet]
    public async Task<IActionResult> GetUserAccounts()
    {
        var userId = GetCurrentUserId();
        var accounts = await _service.GetAccountsByUserIdAsync(userId);
        return Ok(accounts);
    }

    // Sadece giri� yapm�� kullan�c�n�n belirli bir hesab�n� getirir
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccountById(int id)
    {
        var userId = GetCurrentUserId();
        var account = await _service.GetAccountByIdAsync(id, userId);
        if (account == null)
        {
            return NotFound("Hesap bulunamad� veya bu hesaba eri�im yetkiniz yok.");
        }
        return Ok(account);
    }

    // Giri� yapm�� kullan�c� i�in yeni bir hesap olu�turur
    [HttpPost]
    public async Task<IActionResult> AddAccount([FromBody] CreateAccountDto accountDto)
    {
        if (accountDto == null)
        {
            return BadRequest();
        }

        var account = new Account
        {
            AccountName = accountDto.AccountName,
            AccountType = accountDto.AccountType,
            Currency = accountDto.Currency,
            Balance = accountDto.Balance,
            UserId = GetCurrentUserId()
        };

        var newAccount = await _service.AddAsync(account);
        return CreatedAtAction(nameof(GetAccountById), new { id = newAccount.Id }, newAccount);
    }

    // Sadece giri� yapm�� kullan�c�n�n bir hesab�n� g�nceller
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccount(int id, [FromBody] UpdateAccountDto accountDto)
    {
        if (accountDto == null)
        {
            return BadRequest();
        }

        var userId = GetCurrentUserId();
        // G�ncellenmek istenen hesab�n bu kullan�c�ya ait olup olmad���n� kontrol et
        var existingAccount = await _service.GetAccountByIdAsync(id, userId);
        if (existingAccount == null)
        {
            return NotFound("G�ncellenecek hesap bulunamad� veya bu hesaba eri�im yetkiniz yok.");
        }

        // Mevcut hesab� g�ncelle
        existingAccount.AccountName = accountDto.AccountName;
        existingAccount.AccountType = accountDto.AccountType;
        existingAccount.Currency = accountDto.Currency;
        existingAccount.Balance = accountDto.Balance;

        await _service.UpdateAsync(existingAccount);
        return NoContent();
    }

    // Sadece giri� yapm�� kullan�c�n�n bir hesab�n� siler
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        var userId = GetCurrentUserId();
        // Silinmek istenen hesab�n bu kullan�c�ya ait olup olmad���n� kontrol et
        var accountToDelete = await _service.GetAccountByIdAsync(id, userId);

        if (accountToDelete == null)
        {
            return NotFound("Silinecek hesap bulunamad� veya bu hesaba eri�im yetkiniz yok.");
        }

        await _service.DeleteAsync(accountToDelete);
        return NoContent();
    }
}