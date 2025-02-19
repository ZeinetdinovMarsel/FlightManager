using FM.Core.Enums;
using FM.Core.Models;
using FM.DataAccess.Repositories;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;

    public TicketService(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<IEnumerable<TicketModel>> GetAllTicketsAsync(
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
        return await _ticketRepository.GetAllAsync(sortBy, descending, page, pageSize, ticketTypeFilter, priceFilter, seatFilter, flightIdFilter, serviceIdsFilter);
    }

    public async Task<TicketModel?> GetTicketByIdAsync(int id)
    {
        return await _ticketRepository.GetByIdAsync(id);
    }

    public async Task<int> CreateTicketAsync(TicketModel ticketModel)
    {
        return await _ticketRepository.CreateAsync(ticketModel);
    }

    public async Task<bool> UpdateTicketAsync(int id, TicketModel ticketModel)
    {
        return await _ticketRepository.UpdateAsync(id, ticketModel);
    }

    public async Task<bool> DeleteTicketAsync(int id)
    {
        return await _ticketRepository.DeleteAsync(id);
    }
}
