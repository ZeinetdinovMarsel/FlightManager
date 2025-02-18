using Microsoft.EntityFrameworkCore;
using FM.Core.Models;

namespace FM.DataAccess.Repositories;
public class AirportRepository : IAirportRepository
{
    private readonly FMDbContext _context;

    public AirportRepository(FMDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AirportModel>> GetAllAsync(string? sortBy = null, bool descending = false, int page = 1, int pageSize = 10, string? cityFilter = null, string? nameFilter = null, int? federalDistrictIdFilter = null)
    {
        var query = _context.Airports.AsQueryable();

        if (!string.IsNullOrEmpty(nameFilter))
        {
            query = query.Where(a => a.Name.Contains(nameFilter));
        }

        if (!string.IsNullOrEmpty(cityFilter))
        {
            query = query.Where(a => a.City.Contains(cityFilter));
        }

        if (federalDistrictIdFilter.HasValue)
        {
            query = query.Where(a => a.FederalDistrictId == federalDistrictIdFilter.Value);
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            query = sortBy.ToLower() switch
            {
                "id" => descending ? query.OrderByDescending(a => a.Id) : query.OrderBy(a => a.Id),
                "name" => descending ? query.OrderByDescending(a => a.Name) : query.OrderBy(a => a.Name),
                "city" => descending ? query.OrderByDescending(a => a.City) : query.OrderBy(a => a.City),
                "federaldistrict" => descending ? query.OrderByDescending(a => a.FederalDistrict.Name) : query.OrderBy(a => a.FederalDistrict.Name),

                _ => query
            };
        }

        var skip = (page - 1) * pageSize;
        var airports = await query.Skip(skip).Take(pageSize).ToListAsync();

        return airports.Select(a => AirportModel.Create(a.Id, a.Name, a.City, a.FederalDistrictId));
    }


    public async Task<AirportModel?> GetByIdAsync(int id)
    {
        var airport = await _context.Airports
            .FirstOrDefaultAsync(a => a.Id == id);

        if (airport == null) return null;

        return AirportModel.Create(airport.Id, airport.Name, airport.City, airport.FederalDistrictId);
    }

    public async Task<int> CreateAsync(string name, string city, int federalDistrictId)
    {
        var existingDistrict = await _context.FederalDistricts
           .FirstOrDefaultAsync(d => d.Id == federalDistrictId);

        if (existingDistrict == null)
        {
            throw new Exception("Округа с таким id не существует");
        }

        if (city == "" || name=="")
        {
            throw new Exception("Поля не должны быть пустыми");
        }

        var existingAirport = await _context.Airports
           .FirstOrDefaultAsync(a => a.Name == name);

        if (existingAirport != null)
        {
            throw new Exception("Аэропорт с таким именем уже существует");
        }

        var airport = new AirportEntity
        {
            Name = name,
            City = city,
            FederalDistrictId = federalDistrictId,
            FederalDistrict = existingDistrict
        };


        _context.Airports.Add(airport);
        await _context.SaveChangesAsync();
        return airport.Id;
    }

    public async Task<bool> UpdateAsync(int id, string name, string city, int federalDistrictId)
    {
        var district = await _context.FederalDistricts
            .FirstOrDefaultAsync(d => d.Id == federalDistrictId);

        if (district == null)
        {
            throw new Exception("Округа с таким id не существует");
        }

        var airport = await _context.Airports
           .FirstOrDefaultAsync(a => a.Id == id);

        if (airport == null)
        {
            throw new Exception("Аэропорта с таким именем не существует");
        }

        airport.Name = name;
        airport.City = city;
        airport.FederalDistrictId = federalDistrictId;
        airport.FederalDistrict = district;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var airport = await _context.Airports
            .FirstOrDefaultAsync(a => a.Id == id);

        if (airport == null) return false;

        _context.Airports.Remove(airport);
        await _context.SaveChangesAsync();
        return true;
    }
}

