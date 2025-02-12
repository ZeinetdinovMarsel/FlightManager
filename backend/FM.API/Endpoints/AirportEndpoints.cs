using FM.API.Contracts;
using FM.API.Extentions;
using FM.Application.Services;
using FM.Core.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace FM.API.Endpoints;

public static class AirportEndpoints
{
    public static IEndpointRouteBuilder MapAirportEndpoints(
    this IEndpointRouteBuilder app)
    {
        app.MapGet("airports", GetAirports);
        app.MapGet("airports/{id}", GetAirportById);
        app.MapPost("airports", CreateAirport).RequireRoles(Role.Admin);
        app.MapPut("airports/{id}", UpdateAirport).RequireRoles(Role.Admin);
        app.MapDelete("airports/{id}", DeleteAirport).RequireRoles(Role.Admin);

        return app;
    }

    public static async Task<IResult> GetAirports(IAirportService service,
      string? sortBy = null, bool descending = false, int page = 1, int pageSize = 10, string? filter = null)
    {
        try
        {
            var airports = await service.GetAllAsync(sortBy, descending, page, pageSize, filter);
            return Results.Ok(airports);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }


    public static async Task<IResult> GetAirportById(int id, IAirportService service)
    {
        try
        {
            var airport = await service.GetByIdAsync(id);
            return airport != null
                ? Results.Ok(airport)
                : Results.NotFound("Аэропорт не найден");
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public static async Task<IResult> CreateAirport(AirportRequest request, IAirportService service)
    {
        try
        {
            var airportId = await service.CreateAsync(request.Name, request.City, request.FederalDistrictId);
            return Results.Created($"/airports/{airportId}", airportId);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public static async Task<IResult> UpdateAirport(int id, AirportRequest request, IAirportService service)
    {
        try
        {
            var result = await service.UpdateAsync(id, request.Name, request.City, request.FederalDistrictId);
            return result
                ? Results.Ok("Аэропорт обновлён")
                : Results.NotFound("Аэропорт не найден");
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public static async Task<IResult> DeleteAirport(int id, IAirportService service)
    {
        try
        {
            var result = await service.DeleteAsync(id);
            return result
                ? Results.Ok("Аэропорт удалён")
                : Results.NotFound("Аэропорт не найден");
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}
