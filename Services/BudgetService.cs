using PocketBankBE.Data;
using PocketBankBE.Models;

namespace PocketBankBE.Services;

public class BudgetService
{
    private readonly ApplicationDbContext _context;

    public BudgetService(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Budget> GetAll() => _context.Budgets.ToList();

    public Budget? GetById(int id) => _context.Budgets.Find(id);

    public void Add(Budget budget)
    {
        _context.Budgets.Add(budget);
        _context.SaveChanges();
    }

    public void Update(Budget budget)
    {
        _context.Budgets.Update(budget);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var budget = _context.Budgets.Find(id);
        if (budget != null)
        {
            _context.Budgets.Remove(budget);
            _context.SaveChanges();
        }
    }
}