using Microsoft.EntityFrameworkCore;
using FM.Core.Abstractions;
using FM.Core.Enums;
using FM.Core.Models;
using FM.DataAccess.Entities;

namespace FM.DataAccess.Repositories;
public class FlightRepository : IFlightRepository
{
    private readonly FMDbContext _context;

    public FlightRepository(FMDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FlightModel>> GetAllAsync(string? sortBy = null, bool descending = false, int page = 1, int pageSize = 10,
        string? flightNumberFilter = null,
        string? destinationFilter = null,
        DateTime? departureTimeFilter = null,
        DateTime? arrivalTimeFilter = null,
        int? availableSeatsFilter = null,
        int? airoirtIdFilter = null
        )
    {
        var query = _context.Flights.Include(f => f.Airport).AsQueryable();

        if (!string.IsNullOrEmpty(flightNumberFilter))
        {
            query = query.Where(f => f.FlightNumber.Contains(flightNumberFilter));
        }

        if (!string.IsNullOrEmpty(destinationFilter))
        {
            query = query.Where(f => f.Destination.Contains(destinationFilter));
        }

        if (departureTimeFilter.HasValue)
        {
            query = query.Where(f => f.DepartureTime == departureTimeFilter.Value);
        }

        if (arrivalTimeFilter.HasValue)
        {
            query = query.Where(f => f.ArrivalTime == arrivalTimeFilter.Value);
        }

        if (availableSeatsFilter.HasValue)
        {
            query = query.Where(f => f.AvailableSeats == availableSeatsFilter.Value);
        }

        if (airoirtIdFilter.HasValue)
        {
            query = query.Where(f => f.AirportId == airoirtIdFilter.Value);
        }

        if (!string.IsNullOrEmpty(sortBy))
        {
            query = sortBy.ToLower() switch
            {
                "id" => descending ? query.OrderByDescending(f => f.Id) : query.OrderBy(f => f.Id),
                "flightnumber" => descending ? query.OrderByDescending(f => f.FlightNumber) : query.OrderBy(f => f.FlightNumber),
                "destination" => descending ? query.OrderByDescending(f => f.Destination) : query.OrderBy(f => f.Destination),
                "departuretime" => descending ? query.OrderByDescending(f => f.DepartureTime) : query.OrderBy(f => f.DepartureTime),
                "arrivaltime" => descending ? query.OrderByDescending(f => f.ArrivalTime) : query.OrderBy(f => f.ArrivalTime),
                "availableseats" => descending ? query.OrderByDescending(f => f.AvailableSeats) : query.OrderBy(f => f.AvailableSeats),
                "airport" => descending ? query.OrderByDescending(f => f.Airport.Name) : query.OrderBy(f => f.Airport.Name),
                _ => query
            };
        }

        var skip = (page - 1) * pageSize;
        var flights = await query.Skip(skip).Take(pageSize).ToListAsync();

        return flights.Select(f => FlightModel.Create(f.Id, f.FlightNumber, f.Destination, f.DepartureTime, f.ArrivalTime, f.AvailableSeats, f.AirplanePhotoUrl, f.AirportId));
    }


    public async Task<FlightModel?> GetByIdAsync(int id)
    {
        var flight = await _context.Flights.Include(f => f.Airport).FirstOrDefaultAsync(f => f.Id == id);

        if (flight == null) return null;

        return FlightModel.Create(flight.Id, flight.FlightNumber, flight.Destination, flight.DepartureTime, flight.ArrivalTime, flight.AvailableSeats, flight.AirplanePhotoUrl, flight.AirportId);
    }

    public async Task<int> CreateAsync(string flightNumber, string destination, DateTime departureTime, DateTime arrivalTime, int availableSeats, string airplanePhotoUrl, int airportId)
    {
        var airport = await _context.Airports.FirstOrDefaultAsync(a => a.Id == airportId);

        if (airport == null)
        {
            throw new Exception("Аэропорт с таким id не существует");
        }

        var flight = new FlightEntity
        {
            FlightNumber = flightNumber,
            Destination = destination,
            DepartureTime = departureTime,
            ArrivalTime = arrivalTime,
            AvailableSeats = availableSeats,
            AirplanePhotoUrl = airplanePhotoUrl,
            AirportId = airportId,
            Airport = airport
        };

        _context.Flights.Add(flight);
        await _context.SaveChangesAsync();
        return flight.Id;
    }

    public async Task<bool> UpdateAsync(int id, string flightNumber, string destination, DateTime departureTime, DateTime arrivalTime, int availableSeats, string airplanePhotoUrl, int airportId)
    {
        var airport = await _context.Airports.FirstOrDefaultAsync(a => a.Id == airportId);

        if (airport == null)
        {
            throw new Exception("Аэропорт с таким id не существует");
        }

        var flight = await _context.Flights.FirstOrDefaultAsync(f => f.Id == id);

        if (flight == null)
        {
            throw new Exception("Рейс с таким id не существует");
        }

        flight.FlightNumber = flightNumber;
        flight.Destination = destination;
        flight.DepartureTime = departureTime;
        flight.ArrivalTime = arrivalTime;
        flight.AvailableSeats = availableSeats;
        flight.AirplanePhotoUrl = airplanePhotoUrl;
        flight.AirportId = airportId;
        flight.Airport = airport;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var flight = await _context.Flights.FirstOrDefaultAsync(f => f.Id == id);

        if (flight == null) return false;

        _context.Flights.Remove(flight);
        await _context.SaveChangesAsync();
        return true;
    }
}