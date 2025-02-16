using FM.Core.Enums;
using FM.Core.Models;

namespace FM.Core.Abstractions;
public interface IUsersService
{
    Task<List<UserModel>> GetAllUsers();
    Task<List<UserModel>> GetAllUsersByRole(int role);
    Task<UserModel> GetUserFromToken(string token);
    Task<Role> GetUserRole(Guid id);
    Task<string> SignIn(string email, string password);
    Task<Guid> SignUp(string userName, string email, string password, int role);
}