using PocketBankBE.Models;

namespace PocketBankBE.Services;

public interface IJwtService
{
    string GenerateToken(User user);
}