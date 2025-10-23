using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PocketBankBE.Data;
using PocketBankBE.DTOs;
using PocketBankBE.Models;
using PocketBankBE.Services;
using BCrypt.Net;

namespace PocketBankBE.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthController(ApplicationDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
    {
        // Check if user already exists
        if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
        {
            return BadRequest("User with this email already exists");
        }

        // Hash password
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        // Create new user
        var user = new User
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            PasswordHash = hashedPassword,
            PhoneNumber = registerDto.PhoneNumber,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Generate JWT token
        var token = _jwtService.GenerateToken(user);
        var expiresAt = DateTime.UtcNow.AddHours(24);

        var response = new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ExpiresAt = expiresAt
        };

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        // Find user by email
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        
        if (user == null)
        {
            return BadRequest("Invalid email or password");
        }

        // Verify password
        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            return BadRequest("Invalid email or password");
        }

        // Check if user is active
        if (!user.IsActive)
        {
            return BadRequest("Account is deactivated");
        }

        // Generate JWT token
        var token = _jwtService.GenerateToken(user);
        var expiresAt = DateTime.UtcNow.AddHours(24);

        var response = new AuthResponseDto
        {
            Token = token,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ExpiresAt = expiresAt
        };

        return Ok(response);
    }
}