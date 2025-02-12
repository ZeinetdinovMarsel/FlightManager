using FM.API.Contracts;
using FM.Application.Services;

namespace FM.API.Endpoints;
public static class UsersEndpoints
{

    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("register", Register);

        app.MapPost("login", Login);

        app.MapPost("logout", Logout);

        app.MapGet("user", GetUserDetails).RequireAuthorization();

        app.MapGet("users", GetUsersByRole).RequireAuthorization();

        app.MapGet("user/role", GetUserRole).RequireAuthorization();
        return app;
    }
    private static async Task<IResult> Register(
        RegisterUserRequest request,
        UsersService usersService)
    {

        try
        {

            var userId = await usersService.Register(
                request.UserName,
                request.Email,
                request.Password,
                request.Role);
            return Results.Ok(userId);
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { Message = ex.Message });
        }

    }

    private static async Task<IResult> Login(
        LoginUserRequest request,
        UsersService usersService)
    {
        try
        {
            var token = await usersService.Login(request.Email, request.Password);
            return Results.Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { Message = ex.Message });
        }
    }

    private static IResult Logout()
    {
        return Results.Ok(new { Message = "Вы успешно вышли" });
    }

    private static async Task<IResult> GetUserDetails(
        UsersService usersService,
        HttpContext context)
    {
        try
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
                return Results.Unauthorized();

            var user = await usersService.GetUserFromToken(token);
            if (user == null)
                return Results.NotFound(new { Message = "Пользователь не найден" });

            return Results.Ok(user);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { Message = ex.Message });
        }
    }

    private static async Task<IResult> GetUsersByRole(
      int roleId,
       UsersService usersService

      )
    {
        var users = await usersService.GetAllUsersByRole(roleId);

        var response = users.Select(u => new UsersRequest(u.Id, u.UserName));

        return Results.Ok(response);
    }

    private static async Task<IResult> GetUserRole(
        UsersService usersService,
        HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var user = await usersService.GetUserFromToken(token);

        var role = await usersService.GetUserRole(user.Id);

        return Results.Ok(role);
    }
}