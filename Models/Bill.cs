using System.ComponentModel.DataAnnotations;

namespace PocketBankBE.Models;

public class Bill
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string BillName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsPaid { get; set; }
}
