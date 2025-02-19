using FM.Core.Models;

namespace FM.Application.Services
{
    public interface IServiceService
    {
        Task<int> CreateAsync(string name, decimal cost);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ServiceModel>> GetAllAsync(string? sortBy = null, bool descending = false, int page = 1, int pageSize = 10, string? nameFilter = null, decimal? costFilter = null);
        Task<ServiceModel?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, string name, decimal cost);
    }
}