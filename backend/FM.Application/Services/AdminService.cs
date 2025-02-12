using FM.Core.Abstractions;
using FM.Core.Enums;
using FM.Core.Models;

namespace FM.Application.Services;
public class AdminService
{
    private readonly IAdminRepository _adminRepository;

    public AdminService(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }

    public async Task<Guid> UpdateUser(Guid id, UserModel user, Role role)
    {
        return await _adminRepository.Update(id, user, role);
    }

    public async Task<Guid> DeleteUser(Guid id)
    {
        return await _adminRepository.Delete(id);
    }
}

