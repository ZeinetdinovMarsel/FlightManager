using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FM.API.Endpoints;
using FM.Application;
using FM.Core.Abstractions;
using FM.Core.Enums;
using FM.Infrastructure;
using FM.Application.Services;
namespace FM.API.Extentions;
public static class ApiExtentions
{
    public static void AddMappedEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapUsersEndpoints();
        app.MapAdminEndpoints();
        app.MapFederalDistrictEndpoints();
        app.MapAirportEndpoints();
        app.MapFlightEndpoints();
        app.MapTicketEndpoints();
        app.MapServiceEndpoints();
    }
    public static IServiceCollection AddApiAuthentification(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                        if (authHeader?.StartsWith("Bearer ") == true)
                        {
                            context.Token = authHeader.Substring("Bearer ".Length).Trim();
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddScoped<IPermissionService, PermissionService>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddScoped<IRoleService, RoleService>();
        services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler>();
        services.AddAuthorization();
        return services;
    }


    public static IEndpointConventionBuilder RequirePermissions<TBuilder>(
        this TBuilder builder, params Permission[] permissions)
        where TBuilder : IEndpointConventionBuilder
    {
        return builder.RequireAuthorization(policy =>
        policy.AddRequirements(new PermissionRequirement(permissions)));
    }

    public static IEndpointConventionBuilder RequireRoles<TBuilder>(
        this TBuilder builder, params Role[] roles)
        where TBuilder : IEndpointConventionBuilder
    {
        return builder.RequireAuthorization(policy =>
        policy.AddRequirements(new RoleRequirement(roles)));
    }
}
