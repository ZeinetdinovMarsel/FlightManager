using FM.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using FM.Core.Models;

namespace FM.DataAccess.Repositories;
public class FederalDistrictRepository : IFederalDistrictRepository
{
    private readonly FMDbContext _context;

    public FederalDistrictRepository(FMDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FederalDistrictModel>> GetAllAsync(string? sortBy = null, bool descending = false, int page = 1, int pageSize = 10, string? filter = null)
    {
        var query = _context.FederalDistricts.AsQueryable();

        if (!string.IsNullOrEmpty(filter))
        {
            query = query.Where(d => d.Name.Contains(filter));
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            query = sortBy.ToLower() switch
            {
                "name" => descending ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name),
                _ => query
            };
        }

        var skip = (page - 1) * pageSize;
        var districts = await query.Skip(skip).Take(pageSize).ToListAsync();

        return districts.Select(d => FederalDistrictModel.Create(d.Id, d.Name));
    }


    public async Task<FederalDistrictModel?> GetByIdAsync(int id)
    {
        var district = await _context.FederalDistricts
            .FirstOrDefaultAsync(d => d.Id == id);

        if (district == null) return null;

        return FederalDistrictModel.Create(district.Id, district.Name);
    }

    public async Task<int> CreateAsync(string name)
    {
        var district = new FederalDistrictEntity
        {
            Name = name
        };

        var existingDistrict = await _context.FederalDistricts
            .FirstOrDefaultAsync(d => d.Name == name);

        if (existingDistrict != null)
        {
            throw new Exception("Округ с таким именем уже существует");
        }

        _context.FederalDistricts.Add(district);
        await _context.SaveChangesAsync();
        return district.Id;
    }

    public async Task<bool> UpdateAsync(int id, string name)
    {
        var district = await _context.FederalDistricts
            .FirstOrDefaultAsync(d => d.Id == id);

        if (district == null) return false;

        district.Name = name;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var district = await _context.FederalDistricts
            .FirstOrDefaultAsync(d => d.Id == id);

        if (district == null) return false;

        _context.FederalDistricts.Remove(district);
        await _context.SaveChangesAsync();
        return true;
    }
}
