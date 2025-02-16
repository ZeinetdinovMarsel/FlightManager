using FM.Core.Abstractions;
using FM.Core.Enums;
using FM.DataAccess.Repositories;

namespace FM.Application;

public class PermissionService :  IPermissionService
{
    private readonly IUsersRepository _usersRepository;

    public PermissionService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }
    public Task<HashSet<Permission>> GetPermissionsAsync(Guid userId)
    {

        return _usersRepository.GetUserPermissionsAsync(userId);
    }
}