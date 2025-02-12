
public interface ITicketService
{
    Task<int> CreateTicketAsync(TicketModel ticketModel);
    Task<bool> DeleteTicketAsync(int id);
    Task<IEnumerable<TicketModel>> GetAllTicketsAsync(string? sortBy = null, bool descending = false, int page = 1, int pageSize = 10, string? filter = null);
    Task<TicketModel?> GetTicketByIdAsync(int id);
    Task<bool> UpdateTicketAsync(int id, TicketModel ticketModel);
}