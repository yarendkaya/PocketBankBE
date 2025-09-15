using Microsoft.EntityFrameworkCore; // Async metotlar i�in bu using gerekli
using PocketBankBE.Data;
using PocketBankBE.Models;

namespace PocketBankBE.Services;

public class AccountService
{
    private readonly ApplicationDbContext _context;

    public AccountService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Not: B�t�n metotlar� daha performansl� �al��malar� i�in asenkron (async) hale getirdik.
    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        return await _context.Accounts.ToListAsync();
    }

    public async Task<Account?> GetByIdAsync(int id)
    {
        return await _context.Accounts.FindAsync(id);
    }

    public async Task<Account> AddAsync(Account account)
    {
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();
        return account;
    }

    public async Task UpdateAsync(Account account)
    {
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Account account)
    {
        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Account>> GetAccountsByUserIdAsync(int userId)
    {
        return await _context.Accounts
            .Where(a => a.UserId == userId)
            .ToListAsync();
    }

    public async Task<Account?> GetAccountByIdAsync(int id, int userId)
    {
        return await _context.Accounts
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
    }
}