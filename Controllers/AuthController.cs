using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PocketBankBE.Data;
using PocketBankBE.DTOs;
using PocketBankBE.Models;
using PocketBankBE.Services;
using BCrypt.Net;

namespace PocketBankBE.Controllers
{
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
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                return BadRequest("User with this email already exists");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                PasswordHash = hashedPassword,
                PhoneNumber = registerDto.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                IsActive = true // Yeni kullan�c�y� varsay�lan olarak aktif yapal�m
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

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

            // --- D�ZELTME BA�LANGICI ---
            // ��kmeyi �nlemek i�in, �ifre kontrol�nden �nce kullan�c�n�n
            // var oldu�undan VE bir �ifre hash'ine sahip oldu�undan emin olmal�y�z.
            if (user == null || string.IsNullOrEmpty(user.PasswordHash))
            {
                // Kullan�c� bulunamazsa veya �ifresi yoksa, genel bir hata mesaj� d�nd�r.
                // Bu, "kullan�c� ad� ge�erli ama �ifre de�il" gibi ipu�lar� vermeyi engeller.
                return BadRequest("Invalid email or password");
            }
            // --- D�ZELTME B�T��� ---

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

}
