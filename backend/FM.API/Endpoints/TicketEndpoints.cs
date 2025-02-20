using FM.API.Contracts;
using FM.API.Extentions;
using FM.Application.Services;
using FM.Core.Enums;
using FM.Core.Models;

namespace FM.API.Endpoints;
public static class TicketEndpoints
{
    public static IEndpointRouteBuilder MapTicketEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/tickets", GetTickets);
        app.MapGet("/tickets/{id}", GetTicketById);
        app.MapPost("/tickets", CreateTicket);
        app.MapPut("/tickets/{id}", UpdateTicket);
        app.MapDelete("/tickets/{id}", DeleteTicket);
        app.MapGet("/ticket-types", GetTicketTypes);
        return app;
    }

    private static async Task<IResult> GetTickets(TicketService service,
    string? sortBy = null,
    bool descending = false,
    int page = 1,
    int pageSize = 10,
    int? ticketTypeFilter = null,
    float? priceFilter = null,
    string? seatFilter = null,
    int? flightIdFilter = null,
    int[]? serviceIdsFilter = null
)
    {
        try
        {
            var tickets = await service.GetAllTicketsAsync(sortBy, descending, page, pageSize, ticketTypeFilter, priceFilter, seatFilter, flightIdFilter,serviceIdsFilter);
            return Results.Ok(tickets);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }


    private static async Task<IResult> GetTicketById(int id, TicketService service)
    {
        try
        {
            var ticket = await service.GetTicketByIdAsync(id);
            return ticket != null ? Results.Ok(ticket) : Results.NotFound();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private static async Task<IResult> CreateTicket(TicketRequest request, TicketService service)
    {
        try
        {
            var ticketModel = TicketModel.Create(
                0,
                request.TicketType,
                request.Price,
                request.Seat,
                request.FlightId,
                request.Services.Select(s => TicketServiceModel.Create(0, s.ServiceId, 0)).ToList()
            );

            var ticketId = await service.CreateTicketAsync(ticketModel);
            return Results.Ok(new { TicketId = ticketId });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private static async Task<IResult> UpdateTicket(int id, TicketRequest request, TicketService service)
    {
        try
        {
            var ticketModel = TicketModel.Create(
                id,
                request.TicketType,
                request.Price,
                request.Seat,
                request.FlightId,
                request.Services.Select(s => TicketServiceModel.Create(0, s.ServiceId, id)).ToList()
            );

            var result = await service.UpdateTicketAsync(id, ticketModel);
            return result ? Results.Ok("Билет обновлён") : Results.NotFound();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private static async Task<IResult> DeleteTicket(int id, TicketService service)
    {
        try
        {
            var result = await service.DeleteTicketAsync(id);
            return result ? Results.Ok("Билет удалён") : Results.NotFound();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private static IResult GetTicketTypes()
    {
        try
        {
            var ticketTypes = Enum.GetValues(typeof(TicketType))
                                   .Cast<TicketType>()
                                   .Select(t => new
                                   {
                                       Id = (int)t,
                                       Name = t.ToString()
                                   })
                                   .ToList();

            return Results.Ok(ticketTypes);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}
