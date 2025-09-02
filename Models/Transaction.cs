using System.ComponentModel.DataAnnotations;

namespace PocketBankBE.Models;

public class Transaction
{
    [Key]
    public int Id { get; set; }
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsIncome { get; set; }
}