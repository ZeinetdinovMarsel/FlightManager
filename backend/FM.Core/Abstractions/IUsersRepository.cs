using FM.Core.Enums;
using FM.Core.Models;

namespace FM.DataAccess.Repositories;
public interface IUsersRepository
{
    Task AddAsync(UserModel user, int role);
    Task<UserModel?> GetByEmailAsync(string email);
    Task<UserModel> GetByIdAsync(Guid id);
    Task<UserModel?> GetUserByRefreshTokenAsync(string refreshToken);
    Task<HashSet<Permission>> GetUserPermissionsAsync(Guid userId);
    Task<List<Role>> GetUserRolesAsync(Guid userId);
    Task<List<UserModel>> GetUsersAsync();
    Task<List<UserModel>> GetUsersByRoleAsync(int role);
    Task RevokeRefreshTokenAsync(Guid userId);
    Task UpdateRefreshTokenAsync(Guid userId, string refreshToken);
    Task UpdateUser(UserModel? user);
}
