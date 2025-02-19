using FM.Core.Models;
using FM.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace FM.DataAccess.Repositories;
public class ServiceRepository : IServiceRepository
{
    private readonly FMDbContext _context;

    public ServiceRepository(FMDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ServiceModel>> GetAllAsync(string? sortBy = null, bool descending = false, int page = 1, int pageSize = 10,
        string? nameFilter = null,
        decimal? costFilter = null)
    {
        var query = _context.Services.AsQueryable();

        if (!string.IsNullOrEmpty(nameFilter))
        {
            query = query.Where(s => s.Name.Contains(nameFilter));
        }

        if (costFilter.HasValue)
        {
            query = query.Where(s => s.Cost == costFilter.Value);
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            query = sortBy.ToLower() switch
            {
                "id" => descending ? query.OrderByDescending(s => s.Id) : query.OrderBy(s => s.Id),
                "name" => descending ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name),
                "cost" => descending ? query.OrderByDescending(s => s.Cost) : query.OrderBy(s => s.Cost),
                _ => query
            };
        }

        var skip = (page - 1) * pageSize;
        var services = await query.Skip(skip).Take(pageSize).ToListAsync();

        return services.Select(s => ServiceModel.Create(s.Id, s.Name, s.Cost));
    }

    public async Task<ServiceModel?> GetByIdAsync(int id)
    {
        var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);

        if (service == null) return null;

        return ServiceModel.Create(service.Id, service.Name, service.Cost);
    }

    public async Task<int> CreateAsync(string name, decimal cost)
    {
        var service = new ServiceEntity
        {
            Name = name,
            Cost = cost
        };

        _context.Services.Add(service);
        await _context.SaveChangesAsync();
        return service.Id;
    }

    public async Task<bool> UpdateAsync(int id, string name, decimal cost)
    {
        var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);

        if (service == null)
        {
            throw new Exception("Сервис с таким id не существует");
        }

        service.Name = name;
        service.Cost = cost;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);

        if (service == null) return false;

        _context.Services.Remove(service);
        await _context.SaveChangesAsync();
        return true;
    }
}
