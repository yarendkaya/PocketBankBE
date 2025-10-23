using System.ComponentModel.DataAnnotations;

namespace PocketBankBE.DTOs;

public class CreateSavingGoalDto
{
    [Required]
    [StringLength(100)]
    public string GoalName { get; set; } = string.Empty;

    [Required]
    public decimal TargetAmount { get; set; }

    public decimal CurrentAmount { get; set; } = 0.00m;

    [Required]
    public DateTime TargetDate { get; set; }
}

public class UpdateSavingGoalDto
{
    [Required]
    [StringLength(100)]
    public string GoalName { get; set; } = string.Empty;

    [Required]
    public decimal TargetAmount { get; set; }

    [Required]
    public decimal CurrentAmount { get; set; }

    [Required]
    public DateTime TargetDate { get; set; }
}
