using FM.Core.Models;
using FM.DataAccess.Repositories;

namespace FM.Application.Services;
public class AirportService : IAirportService
{
    private readonly IAirportRepository _airportRepository;

    public AirportService(IAirportRepository airportRepository)
    {
        _airportRepository = airportRepository;
    }

    public async Task<IEnumerable<AirportModel>> GetAllAsync(string? sortBy = null, bool descending = false, int page = 1, int pageSize = 10, string? filter = null)
    {
        return await _airportRepository.GetAllAsync(sortBy, descending, page, pageSize, filter);
    }

    public async Task<AirportModel?> GetByIdAsync(int id)
    {
        return await _airportRepository.GetByIdAsync(id);
    }

    public async Task<int> CreateAsync(string name, string city, int federalDistrictId)
    {
        return await _airportRepository.CreateAsync(name, city, federalDistrictId);
    }

    public async Task<bool> UpdateAsync(int id, string name, string city, int federalDistrictId)
    {
        return await _airportRepository.UpdateAsync(id, name, city, federalDistrictId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _airportRepository.DeleteAsync(id);
    }
}
