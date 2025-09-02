using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PocketBankBE.Data;
using System.Security.Claims;

namespace PocketBankBE.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Bu controller'daki tüm metotlar için yetkilendirme gerekir
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        // Token'dan gelen kullanýcý kimliðini al
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return BadRequest("Geçersiz kullanýcý kimliði.");
        }

        // Asenkron olarak veritabanýndan kullanýcýyý bul
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return NotFound("Kullanýcý bulunamadý.");
        }

        // Sadece gerekli bilgileri döndür (þifre hash'i gibi veriler gönderilmez)
        return Ok(new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber,
            user.Balance,
            user.CreatedAt,
            user.IsActive
        });
    }

    // Yarým kalmýþ GetBalance metodunu tamamladýk ve asenkron yaptýk
    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return BadRequest("Geçersiz kullanýcý kimliði.");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return NotFound("Kullanýcý bulunamadý.");
        }

        // Sadece bakiye bilgisini içeren bir nesne döndür
        return Ok(new { user.Balance });
    }
}