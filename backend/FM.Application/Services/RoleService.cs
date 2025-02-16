using FM.Core.Abstractions;
using FM.Core.Enums;
using FM.DataAccess.Repositories;

namespace FM.Application;

public class RoleService : IRoleService
{
    private readonly IUsersRepository _usersRepository;

    public RoleService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }
    public Task<List<Role>> GetRolesAsync(Guid userId)
    {

        return _usersRepository.GetUserRolesAsync(userId);
    }
}