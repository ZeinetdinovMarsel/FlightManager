
public interface ITicketService
{
    Task<int> CreateTicketAsync(TicketModel ticketModel);
    Task<bool> DeleteTicketAsync(int id);
    Task<IEnumerable<TicketModel>> GetAllTicketsAsync(
    string? sortBy = null,
    bool descending = false,
    int page = 1,
    int pageSize = 10,
    int? ticketTypeFilter = null,
    float? priceFilter = null,
    string? seatFilter = null,
    int? flightIdFilter = null,
    int[]? serviceIdsFilter = null
);
    Task<TicketModel?> GetTicketByIdAsync(int id);
    Task<bool> UpdateTicketAsync(int id, TicketModel ticketModel);
}