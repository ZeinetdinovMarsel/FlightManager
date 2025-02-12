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
    public async Task Add(UserModel user, int role)
    {
        var roleEntity = await _context.Roles
            .SingleOrDefaultAsync(r => r.Id == role)
            ?? throw new InvalidOperationException("Роль не найдена");

        var userEntity = new UserEntity()
        {
            Id = user.Id,
            UserName = user.UserName,
            PasswordHash = user.PasswordHash,
            Email = user.Email,
            Roles = [roleEntity]
        };

        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<UserModel> GetByEmail(string email)
    {
        var userEntity = await _context.Users
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.Email == email);

        if (userEntity == null) return null;

        UserModel user = UserModel.Create(userEntity.Id, userEntity.UserName, userEntity.PasswordHash, email);

        return user;
    }
    public async Task<UserModel> GetById(Guid Id)
    {

        var userEntity = await _context.Users
       .AsNoTracking()
       .FirstOrDefaultAsync(u => u.Id == Id);

        if (userEntity == null) return null;

        UserModel user = UserModel.Create(userEntity.Id, userEntity.UserName, userEntity.PasswordHash, userEntity.Email);

        return user;
    }


    public async Task<HashSet<Permission>> GetUserPermissions(Guid userId)
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
    public async Task<List<UserModel>> GetUsers()

    {
        var userEntitites = await _context.Users
            .AsNoTracking()
            .ToListAsync();

        var Users = userEntitites
            .Select(u => UserModel.Create(u.Id, u.UserName, u.PasswordHash, u.Email))
            .ToList();

        return Users;
    }

    public async Task<List<UserModel>> GetUsersByRole(int role)
    {
        var userEntitites = await _context.Users
            .AsNoTracking()
            .Where(u => u.Roles.Any(r => r.Id == role))
            .ToListAsync();

        var Users = userEntitites
                .Select(u => UserModel.Create(u.Id, u.UserName, u.PasswordHash, u.Email))
                .ToList();

        return Users;
    }

    public async Task<List<Role>> GetUserRoles(Guid userId)
    {
        var userRoles = await _context.Users
            .AsNoTracking()
            .Include(u => u.Roles)
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Roles)
            .Select(r => Enum.Parse<Role>(r.Name))
            .ToListAsync();

        return userRoles;
    }
}