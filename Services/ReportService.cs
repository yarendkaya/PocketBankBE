using PocketBankBE.Data;
using PocketBankBE.Models;

namespace PocketBankBE.Services;

public class ReportService
{
    private readonly ApplicationDbContext _context;

    public ReportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Report> GetAll() => _context.Reports.ToList();

    public Report? GetById(int id) => _context.Reports.Find(id);

    public void Add(Report report)
    {
        _context.Reports.Add(report);
        _context.SaveChanges();
    }

    public void Update(Report report)
    {
        _context.Reports.Update(report);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var report = _context.Reports.Find(id);
        if (report != null)
        {
            _context.Reports.Remove(report);
            _context.SaveChanges();
        }
    }
}