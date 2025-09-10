// Services/AccountService.cs

using Microsoft.EntityFrameworkCore;
using PocketBankBE.Data; // DbContext'inizin bulunduðu klasör
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

        // Controller'ýn istediði GetAccountsByUserIdAsync metodu
        public async Task<List<Account>> GetAccountsByUserIdAsync(int userId)
        {
            return await _context.Accounts
                                 .Where(a => a.UserId == userId)
                                 .ToListAsync();
        }

        // Controller'ýn istediði GetAccountByIdAsync metodu (Güvenlik için UserId de kontrol ediliyor)
        public async Task<Account?> GetAccountByIdAsync(int id, int userId)
        {
            return await _context.Accounts
                                 .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
        }

        // Controller'ýn istediði AddAsync metodu (Oluþturulan hesabý geri döndürüyor)
        public async Task<Account> AddAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        // Controller'ýn istediði UpdateAsync metodu
        public async Task UpdateAsync(Account accountToUpdate)
        {
            _context.Entry(accountToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Controller'ýn istediði DeleteAsync metodu
        public async Task DeleteAsync(Account accountToDelete)
        {
            _context.Accounts.Remove(accountToDelete);
            await _context.SaveChangesAsync();
        }
    }
}