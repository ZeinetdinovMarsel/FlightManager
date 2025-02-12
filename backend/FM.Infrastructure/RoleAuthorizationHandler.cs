using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using FM.Core.Abstractions;


namespace FM.Infrastructure;
public class RoleAuthorizationHandler : AuthorizationHandler<RoleRequirement>
{

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RoleAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        RoleRequirement requirement)
    {
        var userId = context.User.Claims.FirstOrDefault(
         c => c.Type == CustomClaims.UserId);

        if (userId is null || !Guid.TryParse(userId.Value, out var id))
        {
            return;
        }
        using var scope = _serviceScopeFactory.CreateScope();

        var roleService = scope.ServiceProvider
        .GetRequiredService<IRoleService>();

        var roles = await roleService.GetRolesAsync(id);
        if (roles.Intersect(requirement.Roles).Any())
        {
            {
                context.Succeed(requirement);
            }
        }
    }
}

