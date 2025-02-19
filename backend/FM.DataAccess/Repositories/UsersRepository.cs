using FM.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using FM.Core.Abstractions;
using FM.Core.Enums;
using System.Data;
using FM.Core.Models;

namespace FM.DataAccess.Repositories;
public class UsersRepository : IUsersRepository
{
    private readonly FMDbContext _context;

    public UsersRepository(FMDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(UserModel user, int role)
    {
        var roleEntity = await _context.Roles.FindAsync(role)
            ?? throw new InvalidOperationException("Роль не найдена");

        var userEntity = new UserEntity
        {
            Id = user.Id,
            UserName = user.UserName,
            PasswordHash = user.PasswordHash,
            Email = user.Email,
            RefreshToken = user.RefreshToken,
            Roles = [roleEntity]
        };

        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<UserModel?> GetByEmailAsync(string email)
    {
        var userEntity = await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

        return userEntity == null ? null : MapToModel(userEntity);
    }

    public async Task<UserModel> GetByIdAsync(Guid id)
    {
        var userEntity = await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
        if (userEntity == null)
            throw new Exception("Пользователь не найден");
        return MapToModel(userEntity);
    }

    public async Task<UserModel?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        var userEntity = await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

        return userEntity != null ? MapToModel(userEntity) : null;
    }

    public async Task UpdateRefreshTokenAsync(Guid userId, string refreshToken)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return;

        user.RefreshToken = refreshToken;

        await _context.SaveChangesAsync();
    }

    public async Task RevokeRefreshTokenAsync(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return;

        user.RefreshToken = null;

        await _context.SaveChangesAsync();
    }

    public async Task<List<UserModel>> GetUsersAsync()
    {
        var users = await _context.Users.AsNoTracking().ToListAsync();
        return users.Select(MapToModel).ToList();
    }

    public async Task<List<UserModel>> GetUsersByRoleAsync(int role)
    {
        var users = await _context.Users.AsNoTracking()
            .Where(u => u.Roles.Any(r => r.Id == role))
            .ToListAsync();

        return users.Select(MapToModel).ToList();
    }
    public async Task<HashSet<Permission>> GetUserPermissionsAsync(Guid userId)
    {
        var roles = await _context.Users
            .AsNoTracking()
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .Where(u => u.Id == userId)
            .Select(u => u.Roles)
            .ToArrayAsync();

        return roles
            .SelectMany(r => r)
            .SelectMany(r => r.Permissions)
            .Select(p => (Permission)p.Id)
            .ToHashSet();
    }
    public async Task<List<Role>> GetUserRolesAsync(Guid userId)
    {
        return await _context.Users.AsNoTracking()
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Roles)
            .Select(r => Enum.Parse<Role>(r.Name))
            .ToListAsync();
    }

    private static UserModel MapToModel(UserEntity entity)
    {
        return UserModel.Create(
            entity.Id,
            entity.UserName,
            entity.PasswordHash,
            entity.Email,
            entity.RefreshToken
        );
    }

    public async Task UpdateUser(UserModel? user)
    {
        if (user == null)
            throw new Exception("Пользователя нет");
        var userEntity = await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == user.Id);
        if (userEntity == null)
            throw new Exception("Пользователь не найден");
        userEntity.Email = user.Email;
        userEntity.UserName = user.UserName;
        userEntity.PasswordHash = user.PasswordHash;
        userEntity.RefreshToken = user.RefreshToken;

        await _context.SaveChangesAsync();
    }
}


