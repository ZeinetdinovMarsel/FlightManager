using FM.Core.Abstractions;
using FM.Core.Enums;
using FM.Core.Models;
using FM.DataAccess.Repositories;

namespace FM.Application.Services;
public class ServiceService : IServiceService
{
    private readonly IServiceRepository _serviceRepository;

    public ServiceService(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<IEnumerable<ServiceModel>> GetAllAsync(string? sortBy = null, bool descending = false, int page = 1, int pageSize = 10, string? nameFilter = null, decimal? costFilter = null)
    {
        return await _serviceRepository.GetAllAsync(sortBy, descending, page, pageSize, nameFilter, costFilter);
    }

    public async Task<ServiceModel?> GetByIdAsync(int id)
    {
        return await _serviceRepository.GetByIdAsync(id);
    }

    public async Task<int> CreateAsync(string name, decimal cost)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Название услуги не может быть пустым", nameof(name));
        }

        if (cost < 0)
        {
            throw new ArgumentException("Стоимость услуги не может быть отрицательной", nameof(cost));
        }

        return await _serviceRepository.CreateAsync(name, cost);
    }

    public async Task<bool> UpdateAsync(int id, string name, decimal cost)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Название услуги не может быть пустым", nameof(name));
        }

        if (cost < 0)
        {
            throw new ArgumentException("Стоимость услуги не может быть отрицательной", nameof(cost));
        }

        return await _serviceRepository.UpdateAsync(id, name, cost);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _serviceRepository.DeleteAsync(id);
    }
}

