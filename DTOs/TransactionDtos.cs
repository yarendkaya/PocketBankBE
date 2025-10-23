using System.ComponentModel.DataAnnotations;

namespace PocketBankBE.DTOs;

public class CreateTransactionDto
{
    [Required]
    public int AccountId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    [StringLength(100)]
    public string Category { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public bool IsIncome { get; set; }
}

public class UpdateTransactionDto
{
    [Required]
    public int AccountId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    [StringLength(100)]
    public string Category { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public bool IsIncome { get; set; }
}
