using FM.Core.Abstractions;
using FM.Core.Enums;
using FM.Core.Models;

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
        var existingUser = await _usersRepository.GetByEmail(email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Почта уже занята");
        }

        var hashedPassword = _passwordHasher.Generate(password);

        var user = UserModel.Create(
            Guid.NewGuid(),
            userName,
            hashedPassword,
            email);

        await _usersRepository.Add(user, role);

        return user.Id;
    }

    public async Task<string> SignIn(string email, string password)
    {
        var user = await _usersRepository.GetByEmail(email);

        if (user == null)
        {
            throw new Exception("Пользователь не найден");
        }
        var result = _passwordHasher.Verify(password, user.PasswordHash);

        if (!result)
        {
            throw new Exception("Не правильный пароль");
        }


        var token = _jwtProvider.GenerateToken(user);
        return token;
    }
    public async Task<UserModel> GetUserFromToken(string token)
    {
        Guid userId = _jwtProvider.ValidateToken(token);

        var user = await _usersRepository.GetById(userId);

        return user;
    }

    public async Task<List<UserModel>> GetAllUsersByRole(int role)
    {
        return await _usersRepository.GetUsersByRole(role);
    }

    public async Task<List<UserModel>> GetAllUsers()
    {
        return await _usersRepository.GetUsers();
    }
    public async Task<Role> GetUserRole(Guid id)
    {
        return (await _usersRepository.GetUserRoles(id))[0];
    }
}