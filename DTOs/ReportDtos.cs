using System.ComponentModel.DataAnnotations;

namespace PocketBankBE.DTOs;

public class CreateReportDto
{
    [Required]
    [StringLength(100)]
    public string Type { get; set; } = string.Empty;

    [StringLength(500)]
    public string FilePath { get; set; } = string.Empty;
}

public class UpdateReportDto
{
    [Required]
    [StringLength(100)]
    public string Type { get; set; } = string.Empty;

    [StringLength(500)]
    public string FilePath { get; set; } = string.Empty;
}
