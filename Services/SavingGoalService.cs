using PocketBankBE.Data;
using PocketBankBE.Models;

namespace PocketBankBE.Services;

public class SavingGoalService
{
    private readonly ApplicationDbContext _context;

    public SavingGoalService(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<SavingGoal> GetAll() => _context.SavingGoals.ToList();

    public SavingGoal? GetById(int id) => _context.SavingGoals.Find(id);

    public void Add(SavingGoal goal)
    {
        _context.SavingGoals.Add(goal);
        _context.SaveChanges();
    }

    public void Update(SavingGoal goal)
    {
        _context.SavingGoals.Update(goal);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var goal = _context.SavingGoals.Find(id);
        if (goal != null)
        {
            _context.SavingGoals.Remove(goal);
            _context.SaveChanges();
        }
    }
}