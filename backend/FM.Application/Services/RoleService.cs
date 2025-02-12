using FM.Core.Abstractions;
using FM.Core.Enums;

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

        return _usersRepository.GetUserRoles(userId);
    }
}