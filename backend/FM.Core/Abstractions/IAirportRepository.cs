using FM.Core.Models;

namespace FM.DataAccess.Repositories
{
    public interface IAirportRepository
    {
        Task<int> CreateAsync(string name, string city, int federalDistrictId);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<AirportModel>> GetAllAsync(string? sortBy = null, bool descending = false, int page = 1, int pageSize = 10, string? filter = null);
        Task<AirportModel?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, string name, string city, int federalDistrictId);
    }
}