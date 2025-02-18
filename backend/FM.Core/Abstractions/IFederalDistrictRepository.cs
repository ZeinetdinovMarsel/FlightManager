using FM.Core.Models;

namespace FM.DataAccess.Repositories;
public interface IFederalDistrictRepository
{
    Task<int> CreateAsync(string name);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<FederalDistrictModel>> GetAllAsync(string? sortBy = null, bool descending = false, int page = 1, int pageSize = 10, string? namefilter = null);
    Task<FederalDistrictModel?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, string name);
}
