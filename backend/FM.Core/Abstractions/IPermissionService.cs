using FM.Core.Enums;

namespace FM.Application;
public interface IPermissionService
{
    Task<HashSet<Permission>> GetPermissionsAsync(Guid userId);
}