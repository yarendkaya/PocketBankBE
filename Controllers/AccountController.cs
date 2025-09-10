// Controllers/AccountController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocketBankBE.Models;
using PocketBankBE.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PocketBankBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _service;

        public AccountController(AccountService service)
        {
            _service = service;
        }

        // GET: api/Account
        [HttpGet]
        public async Task<IActionResult> GetUserAccounts()
        {
            var userId = GetCurrentUserId();
            var accounts = await _service.GetAccountsByUserIdAsync(userId);
            return Ok(accounts);
        }

        // GET: api/Account/5
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

        // POST: api/Account
        [HttpPost]
        public async Task<IActionResult> AddAccount([FromBody] Account account)
        {
            if (account == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            account.UserId = GetCurrentUserId();
            var newAccount = await _service.AddAsync(account);

            return CreatedAtAction(nameof(GetAccountById), new { id = newAccount.Id }, newAccount);
        }

        // PUT: api/Account/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] Account accountToUpdate)
        {
            if (id != accountToUpdate.Id)
            {
                return BadRequest("ID uyuþmazlýðý.");
            }

            var userId = GetCurrentUserId();
            var existingAccount = await _service.GetAccountByIdAsync(id, userId);

            if (existingAccount == null)
            {
                return NotFound("Güncellenecek hesap bulunamadý veya bu hesaba eriþim yetkiniz yok.");
            }

            accountToUpdate.UserId = userId; // Güvenlik için UserId'yi tekrar ata
            await _service.UpdateAsync(accountToUpdate);

            return NoContent();
        }

        // DELETE: api/Account/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var userId = GetCurrentUserId();
            var accountToDelete = await _service.GetAccountByIdAsync(id, userId);

            if (accountToDelete == null)
            {
                return NotFound("Silinecek hesap bulunamadý veya bu hesaba eriþim yetkiniz yok.");
            }

            await _service.DeleteAsync(accountToDelete);
            return NoContent();
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            // Bu durumun normalde yetkilendirme nedeniyle oluþmamasý gerekir.
            throw new System.Exception("Kullanýcý kimliði token içerisinde bulunamadý.");
        }
    }
}