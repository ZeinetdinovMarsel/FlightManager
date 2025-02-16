namespace FM.Core.Models;

public class UserModel
{
    private UserModel(Guid id, string userName, string passwordHash, string email, string refreshToken)
    {
        Id = id;
        UserName = userName;
        PasswordHash = passwordHash;
        Email = email;
        RefreshToken = refreshToken;
    }

    public Guid Id { get; private set; }

    public string UserName { get; private set; }

    public string PasswordHash { get; private set; }

    public string Email { get; private set; }

    public string RefreshToken { get; private set; }

    public static UserModel Create(Guid id, string userName, string passwordHash, string email, string refreshToken)
    {
        return new UserModel(id, userName, passwordHash, email, refreshToken);
    }

    public void UpdateRefreshToken(string newRefreshToken)
    {
        RefreshToken = newRefreshToken;
    }
}
