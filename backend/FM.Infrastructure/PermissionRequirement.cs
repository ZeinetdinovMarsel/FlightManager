using Microsoft.AspNetCore.Authorization;
using FM.Core.Enums;

namespace FM.Infrastructure;
public class PermissionRequirement(Permission[] permissions)
    : IAuthorizationRequirement
{
    public Permission[] Permissions { get; set; } = permissions;
}

