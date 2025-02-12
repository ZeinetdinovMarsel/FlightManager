using FM.Core.Models;

namespace FM.Core.Abstractions;
public interface IJwtProvider
{
    string GenerateToken(UserModel user);
    Guid ValidateToken(string token);
}