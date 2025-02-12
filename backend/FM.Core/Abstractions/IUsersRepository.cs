using FM.Core.Enums;
using FM.Core.Models;

namespace FM.Core.Abstractions;
public interface IUsersRepository
{
    Task Add(UserModel user, int role);
    Task<UserModel> GetByEmail(string email);
    Task<UserModel> GetById(Guid Id);
    Task<List<UserModel>> GetUsersByRole(int role);
    Task<List<UserModel>> GetUsers();
    Task<HashSet<Permission>> GetUserPermissions(Guid userId);
    Task<List<Role>> GetUserRoles(Guid userId);
}