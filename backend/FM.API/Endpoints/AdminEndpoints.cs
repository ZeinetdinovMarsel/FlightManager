using FM.API.Contracts;
using FM.API.Extentions;
using FM.Application.Services;
using FM.Core.Enums;
using FM.Core.Models;

namespace FM.API.Endpoints;
public static class AdminEndpoints
{

    public static IEndpointRouteBuilder MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/admin/users", GetUsers).RequireRoles(Role.Admin);
        app.MapPost("/admin/users", CreateUser).RequireRoles(Role.Admin);
        app.MapPut("/admin/users", UpdateUser).RequireRoles(Role.Admin);
        app.MapDelete("/admin/users/{id:guid}", DeleteUser).RequireRoles(Role.Admin);
        return app;
    }
    private static async Task<IResult> GetUsers(UsersService usersService)
    {
        var users = await usersService.GetAllUsers();
        var response = new List<UsersAdminResponse>();

        foreach (var user in users)
        {
            var userRole = await usersService.GetUserRole(user.Id);
            response.Add(new UsersAdminResponse(
                user.Id,
                user.UserName,
                user.PasswordHash,
                user.Email,
                userRole
            ));
        }

        return Results.Ok(response);
    }



    private static async Task<IResult> CreateUser(
        SignUpUserRequest request,
        UsersService usersService)
    {
        try
        {
            var userId = await usersService.SignUp(
                request.UserName,
                request.Email,
                request.Password,
                (int)request.Role);
            return Results.Ok(userId);
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private static async Task<IResult> UpdateUser(
        AdminService adminService,
        UsersAdminRequest request)
    {
        try
        {
            var user = UserModel.Create(
                request.UserId,
                request.UserName,
                request.Password,
                request.Email,
                "");

            var userId = await adminService.UpdateUser(request.UserId, user, request.Role);

            return Results.Ok(userId);
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private static async Task<IResult> DeleteUser(
        AdminService adminService,
        Guid id)
    {
        try
        {
            return Results.Ok(await adminService.DeleteUser(id));
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(ex.Message);
        }

    }
}