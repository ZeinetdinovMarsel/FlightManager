using FM.Core.Enums;
using FM.Core.Models;

namespace FM.Core.Abstractions;
public interface IAdminRepository
{
    Task<Guid> Delete(Guid id);
    Task<Guid> Update(Guid id, UserModel tsk, Role role);
}
