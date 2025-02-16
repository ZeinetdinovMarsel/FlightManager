using FM.Core.Abstractions;
using FM.Core.Enums;
using FM.Core.Models;
using FM.DataAccess.Repositories;
using FM.Infrastructure;

namespace FM.Application.Services;

public class UsersService : IUsersService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUsersRepository _usersRepository;
    private readonly IJwtProvider _jwtProvider;

    public UsersService(IUsersRepository usersRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        _usersRepository = usersRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<Guid> SignUp(string userName, string email, string password, int role)
    {
        var existingUser = await _usersRepository.GetByEmailAsync(email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Почта уже занята");
        }

        var hashedPassword = _passwordHasher.Generate(password);

        var user = UserModel.Create(
            Guid.NewGuid(),
            userName,
            hashedPassword,
            email,
            "");

        await _usersRepository.AddAsync(user, role);

        return user.Id;
    }

    public async Task<(string,string)> SignIn(string email, string password)
    {


        var user = await _usersRepository.GetByEmailAsync(email);

        if (user == null)
        {
            throw new Exception("Пользователь не найден");
        }

        var result = _passwordHasher.Verify(password, user.PasswordHash);
        if (!result)
        {
            throw new Exception("Неправильный пароль");
        }
        string refreshToken = await _jwtProvider.GenerateRefreshToken(user);
        await _usersRepository.UpdateRefreshTokenAsync(user.Id, refreshToken);
        return ( await _jwtProvider.GenerateActivateToken(user), refreshToken);
    }

    public async Task<UserModel> GetUserFromToken(string token)
    {
        Guid userId = _jwtProvider.ValidateToken(token);
        return await _usersRepository.GetByIdAsync(userId);
    }

    public async Task<List<UserModel>> GetAllUsersByRole(int role)
    {
        return await _usersRepository.GetUsersByRoleAsync(role);
    }

    public async Task<List<UserModel>> GetAllUsers()
    {
        return await _usersRepository.GetUsersAsync();
    }

    public async Task<Role> GetUserRole(Guid id)
    {
        return (await _usersRepository.GetUserRolesAsync(id))[0];
    }

    public async Task<UserModel> ValidateRefreshTokenAsync(string refreshToken)
    {
        var user = await _usersRepository.GetUserByRefreshTokenAsync(refreshToken);

        if (user == null || user.RefreshToken != refreshToken)
        {
            return null;
        }

        return user;
    }

    public async Task UpdateRefreshTokenAsync(Guid userId, string newRefreshToken)
    {
        var user = await _usersRepository.GetByIdAsync(userId);

        user?.UpdateRefreshToken(newRefreshToken);
        await _usersRepository.UpdateUser(user);
    }

    public async Task<string> GenerateActivateToken(UserModel user)
    {
        return await _jwtProvider.GenerateActivateToken(user);
    }public async Task<string> GenerateRefreshToken(UserModel user)
    {
        return await _jwtProvider.GenerateRefreshToken(user);
    }
}
