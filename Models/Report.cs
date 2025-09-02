using System.ComponentModel.DataAnnotations;

namespace PocketBankBE.Models;

public class Report
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Type { get; set; } = string.Empty; // Analitik, Özet, vs.
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string FilePath { get; set; } = string.Empty;
}
