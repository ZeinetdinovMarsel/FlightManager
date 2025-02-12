using Microsoft.AspNetCore.Authorization;
using FM.Core.Enums;

namespace FM.Infrastructure;
public class RoleRequirement(Role[] roles)
    : IAuthorizationRequirement
{
    public Role[] Roles { get; set; } = roles;
}

