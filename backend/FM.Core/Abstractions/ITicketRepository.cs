
namespace FM.DataAccess.Repositories;
public interface ITicketRepository
{
    Task<int> CreateAsync(TicketModel ticketModel);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<TicketModel>> GetAllAsync(
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
    Task<TicketModel?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, TicketModel ticketModel);
}
