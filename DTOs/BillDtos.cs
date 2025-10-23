using System.ComponentModel.DataAnnotations;

namespace PocketBankBE.DTOs;

public class CreateBillDto
{
    [Required]
    [StringLength(100)]
    public string BillName { get; set; } = string.Empty;

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    public bool IsPaid { get; set; } = false;
}

public class UpdateBillDto
{
    [Required]
    [StringLength(100)]
    public string BillName { get; set; } = string.Empty;

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    public bool IsPaid { get; set; }
}
