using FM.DataAccess.Repositories;
using FM.Core.Models;

namespace FM.Application.Services;

public class FederalDistrictService : IFederalDistrictService
{
    private readonly IFederalDistrictRepository _repository;

    public FederalDistrictService(IFederalDistrictRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<FederalDistrictModel>> GetAllFederalDistricts(string? sortBy = null, bool descending = false, int page = 1, int pageSize = 10, string? filter = null)
    {
        return await _repository.GetAllAsync(sortBy, descending, page, pageSize, filter);
    }

    public async Task<FederalDistrictModel?> GetFederalDistrictById(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<int> CreateFederalDistrict(string name)
    {
        return await _repository.CreateAsync(name);
    }

    public async Task<bool> UpdateFederalDistrict(int id, string name)
    {
        return await _repository.UpdateAsync(id, name);
    }

    public async Task<bool> DeleteFederalDistrict(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}
