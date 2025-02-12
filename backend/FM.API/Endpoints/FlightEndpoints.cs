using FM.API.Contracts;
using FM.API.Extentions;
using FM.Application.Services;
using FM.Core.Enums;
using FM.Core.Models;

namespace FM.API.Endpoints;
public static class FlightEndpoints
{
    public static IEndpointRouteBuilder MapFlightEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("flights", GetFlights);
        app.MapGet("flights/{id}", GetFlightById);
        app.MapPost("flights", CreateFlight).RequireRoles(Role.Admin);
        app.MapPut("flights/{id}", UpdateFlight).RequireRoles(Role.Admin);
        app.MapDelete("flights/{id}", DeleteFlight).RequireRoles(Role.Admin);

        return app;
    }

    private static async Task<IResult> GetFlights(FlightService service,
             string? sortBy = null, bool descending = false, int page = 1, int pageSize = 10, string? filter = null)
    {
        try
        {
            var flights = await service.GetAllAsync(sortBy, descending, page, pageSize, filter);
            return Results.Ok(flights);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { Message = ex.Message });
        }
    }

    private static async Task<IResult> GetFlightById(int id, FlightService service)
    {
        try
        {
            var flight = await service.GetByIdAsync(id);
            if (flight == null)
                return Results.NotFound(new { Message = "Рейс не найден" });

            return Results.Ok(flight);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { Message = ex.Message });
        }
    }

    private static async Task<IResult> CreateFlight(FlightRequest request, FlightService service)
    {
        try
        {
            var flightId = await service.CreateAsync(request.FlightNumber, request.Destination, request.DepartureTime, request.ArrivalTime, request.AvailableSeats, request.AirplanePhotoUrl, request.AirportId);
            return Results.Ok(new { FlightId = flightId });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { Message = ex.Message });
        }
    }

    private static async Task<IResult> UpdateFlight(int id, FlightRequest request, FlightService service)
    {
        try
        {
            var result = await service.UpdateAsync(id, request.FlightNumber, request.Destination, request.DepartureTime, request.ArrivalTime, request.AvailableSeats, request.AirplanePhotoUrl, request.AirportId);
            if (!result)
                return Results.NotFound(new { Message = "Рейс не найден" });

            return Results.Ok(new { Message = "Рейс обновлен" });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { Message = ex.Message });
        }
    }

    private static async Task<IResult> DeleteFlight(int id, FlightService service)
    {
        try
        {
            var result = await service.DeleteAsync(id);
            if (!result)
                return Results.NotFound(new { Message = "Рейс не найден" });

            return Results.Ok(new { Message = "Рейс удален" });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { Message = ex.Message });
        }
    }
}

