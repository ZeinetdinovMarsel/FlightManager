
namespace FM.DataAccess.Repositories
{
    public interface ITicketRepository
    {
        Task<int> CreateAsync(TicketModel ticketModel);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TicketModel>> GetAllAsync(string? sortBy = null, bool descending = false, int page = 1, int pageSize = 10, string? filter = null);
        Task<TicketModel?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, TicketModel ticketModel);
    }
}