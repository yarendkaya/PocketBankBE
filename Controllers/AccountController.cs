using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocketBankBE.Models;
using PocketBankBE.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PocketBankBE.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Bu controller'a artýk sadece giriþ yapmýþ kullanýcýlar eriþebilir
public class AccountController : ControllerBase
{
    private readonly AccountService _service;

    public AccountController(AccountService service)
    {
        _service = service;
    }

    // Giriþ yapmýþ kullanýcýnýn ID'sini token'dan okur
    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // Gerçek bir uygulamada TryParse daha güvenli olur
        return int.Parse(userIdClaim!);
    }

    // Sadece giriþ yapmýþ kullanýcýnýn hesaplarýný getirir
    [HttpGet]
    public async Task<IActionResult> GetUserAccounts()
    {
        var userId = GetCurrentUserId();
        var accounts = await _service.GetAccountsByUserIdAsync(userId);
        return Ok(accounts);
    }

    // Sadece giriþ yapmýþ kullanýcýnýn belirli bir hesabýný getirir
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccountById(int id)
    {
        var userId = GetCurrentUserId();
        var account = await _service.GetAccountByIdAsync(id, userId);
        if (account == null)
        {
            return NotFound("Hesap bulunamadý veya bu hesaba eriþim yetkiniz yok.");
        }
        return Ok(account);
    }

    // Giriþ yapmýþ kullanýcý için yeni bir hesap oluþturur
    [HttpPost]
    public async Task<IActionResult> AddAccount([FromBody] Account account)
    {
        if (account == null)
        {
            return BadRequest();
        }
        // Hesabýn doðru kullanýcýya atandýðýndan emin ol
        account.UserId = GetCurrentUserId();

        var newAccount = await _service.AddAsync(account);
        return CreatedAtAction(nameof(GetAccountById), new { id = newAccount.Id }, newAccount);
    }

    // Sadece giriþ yapmýþ kullanýcýnýn bir hesabýný günceller
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccount(int id, [FromBody] Account accountToUpdate)
    {
        if (id != accountToUpdate.Id || accountToUpdate == null)
        {
            return BadRequest();
        }

        var userId = GetCurrentUserId();
        // Güncellenmek istenen hesabýn bu kullanýcýya ait olup olmadýðýný kontrol et
        var existingAccount = await _service.GetAccountByIdAsync(id, userId);
        if (existingAccount == null)
        {
            return NotFound("Güncellenecek hesap bulunamadý veya bu hesaba eriþim yetkiniz yok.");
        }

        // UserId'nin deðiþtirilmediðinden emin ol
        accountToUpdate.UserId = userId;

        await _service.UpdateAsync(accountToUpdate);
        return NoContent();
    }

    // Sadece giriþ yapmýþ kullanýcýnýn bir hesabýný siler
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        var userId = GetCurrentUserId();
        // Silinmek istenen hesabýn bu kullanýcýya ait olup olmadýðýný kontrol et
        var accountToDelete = await _service.GetAccountByIdAsync(id, userId);

        if (accountToDelete == null)
        {
            return NotFound("Silinecek hesap bulunamadý veya bu hesaba eriþim yetkiniz yok.");
        }

        await _service.DeleteAsync(accountToDelete);
        return NoContent();
    }
}