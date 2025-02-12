using FM.Core.Enums;

namespace FM.Core.Abstractions;
public interface IRoleService
{
    Task<List<Role>> GetRolesAsync(Guid userId);
}