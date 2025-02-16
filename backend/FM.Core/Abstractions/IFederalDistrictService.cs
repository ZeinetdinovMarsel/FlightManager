using FM.Core.Models;

namespace FM.Application.Services;
public interface IFederalDistrictService
{
    Task<int> CreateFederalDistrict(string name);
    Task<bool> DeleteFederalDistrict(int id);
    Task<IEnumerable<FederalDistrictModel>> GetAllFederalDistricts(string? sortBy = null, bool descending = false, int page = 1, int pageSize = 10, string? filter = null);
    Task<FederalDistrictModel?> GetFederalDistrictById(int id);
    Task<bool> UpdateFederalDistrict(int id, string name);
}
