using System.ComponentModel.DataAnnotations;

namespace PocketBankBE.DTOs;

public class CreateAccountDto
{
    [Required]
    [StringLength(100)]
    public string AccountName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string AccountType { get; set; } = string.Empty;

    [StringLength(3)]
    public string Currency { get; set; } = "TRY";

    public decimal Balance { get; set; } = 0.00m;
}

public class UpdateAccountDto
{
    [Required]
    [StringLength(100)]
    public string AccountName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string AccountType { get; set; } = string.Empty;

    [StringLength(3)]
    public string Currency { get; set; } = "TRY";

    public decimal Balance { get; set; }
}
