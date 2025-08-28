using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PocketBankBE.Data;
using System.Security.Claims;

namespace PocketBankBE.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return BadRequest("Invalid user ID");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        
        if (user == null)
        {
            return NotFound("User not found");
        }

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

    [HttpGet("balance")]
    public async Task<IActionResult> GetBalance()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return BadRequest("Invalid user ID");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        
        if (user == null)
        {
            return NotFound("User not found");
        }

        return Ok(new { Balance = user.Balance });
    }
}