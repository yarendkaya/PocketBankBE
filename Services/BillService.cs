using PocketBankBE.Data;
using PocketBankBE.Models;

namespace PocketBankBE.Services;

public class BillService
{
    private readonly ApplicationDbContext _context;

    public BillService(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Bill> GetAll() => _context.Bills.ToList();

    public Bill? GetById(int id) => _context.Bills.Find(id);

    public void Add(Bill bill)
    {
        _context.Bills.Add(bill);
        _context.SaveChanges();
    }

    public void Update(Bill bill)
    {
        _context.Bills.Update(bill);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var bill = _context.Bills.Find(id);
        if (bill != null)
        {
            _context.Bills.Remove(bill);
            _context.SaveChanges();
        }
    }
}