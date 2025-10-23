using System.ComponentModel.DataAnnotations;

namespace PocketBankBE.Models;

public class Account
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty; // Vadesiz, Vadeli, Kredi Kartı
    public string Currency { get; set; } = "TRY";
    public decimal Balance { get; set; } = 0.00m;
}