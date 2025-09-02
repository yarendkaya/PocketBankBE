using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PocketBankBE.Data;
using System.Security.Claims;

namespace PocketBankBE.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Bu controller'daki t�m metotlar i�in yetkilendirme gerekir
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
        // Token'dan gelen kullan�c� kimli�ini al
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return BadRequest("Ge�ersiz kullan�c� kimli�i.");
        }

        // Asenkron olarak veritaban�ndan kullan�c�y� bul
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return NotFound("Kullan�c� bulunamad�.");
        }

        // Sadece gerekli bilgileri d�nd�r (�ifre hash'i gibi veriler g�nderilmez)
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

    // Yar�m kalm�� GetBalance metodunu tamamlad�k ve asenkron yapt�k
    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return BadRequest("Ge�ersiz kullan�c� kimli�i.");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return NotFound("Kullan�c� bulunamad�.");
        }

        // Sadece bakiye bilgisini i�eren bir nesne d�nd�r
        return Ok(new { user.Balance });
    }
}