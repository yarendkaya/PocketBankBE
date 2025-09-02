using System.ComponentModel.DataAnnotations;

namespace PocketBankBE.Models;

public class Budget
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Limit { get; set; }
    public string Period { get; set; } = "Monthly"; // Monthly, Yearly
}
