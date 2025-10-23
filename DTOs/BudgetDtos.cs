using System.ComponentModel.DataAnnotations;

namespace PocketBankBE.DTOs;

public class CreateBudgetDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public decimal Limit { get; set; }

    [StringLength(50)]
    public string Period { get; set; } = "Monthly";
}

public class UpdateBudgetDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public decimal Limit { get; set; }

    [Required]
    [StringLength(50)]
    public string Period { get; set; } = "Monthly";
}
