// Services/AccountService.cs

using Microsoft.EntityFrameworkCore;
using PocketBankBE.Data; // DbContext'inizin bulundu�u klas�r
using PocketBankBE.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketBankBE.Services
{
    public class AccountService
    {
        private readonly ApplicationDbContext _context;

        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Controller'�n istedi�i GetAccountsByUserIdAsync metodu
        public async Task<List<Account>> GetAccountsByUserIdAsync(int userId)
        {
            return await _context.Accounts
                                 .Where(a => a.UserId == userId)
                                 .ToListAsync();
        }

        // Controller'�n istedi�i GetAccountByIdAsync metodu (G�venlik i�in UserId de kontrol ediliyor)
        public async Task<Account?> GetAccountByIdAsync(int id, int userId)
        {
            return await _context.Accounts
                                 .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
        }

        // Controller'�n istedi�i AddAsync metodu (Olu�turulan hesab� geri d�nd�r�yor)
        public async Task<Account> AddAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        // Controller'�n istedi�i UpdateAsync metodu
        public async Task UpdateAsync(Account accountToUpdate)
        {
            _context.Entry(accountToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Controller'�n istedi�i DeleteAsync metodu
        public async Task DeleteAsync(Account accountToDelete)
        {
            _context.Accounts.Remove(accountToDelete);
            await _context.SaveChangesAsync();
        }
    }
}