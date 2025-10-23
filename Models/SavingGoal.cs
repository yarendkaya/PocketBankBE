using System.ComponentModel.DataAnnotations;

namespace PocketBankBE.Models;

public class SavingGoal
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string GoalName { get; set; } = string.Empty;
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; }
    public DateTime TargetDate { get; set; }
}
