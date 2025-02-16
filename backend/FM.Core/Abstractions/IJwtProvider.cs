using FM.Core.Models;

namespace FM.Infrastructure;
public interface IJwtProvider
{
    Task<string> GenerateActivateToken(UserModel user);
    Task<string> GenerateRefreshToken(UserModel user);
    Guid ValidateToken(string token);
}
