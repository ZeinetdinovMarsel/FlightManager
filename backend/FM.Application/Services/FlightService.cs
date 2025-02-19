using FM.Core.Abstractions;
using FM.Core.Models;
using FM.DataAccess.Repositories;

namespace FM.Application.Services;
public class FlightService : IFlightService
{
    private readonly IFlightRepository _flightRepository;

    public FlightService(IFlightRepository flightRepository)
    {
        _flightRepository = flightRepository;
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
        return await _flightRepository.GetAllAsync(sortBy, descending, page, pageSize, flightNumberFilter,destinationFilter,departureTimeFilter,arrivalTimeFilter,availableSeatsFilter,airoirtIdFilter);
    }

    public async Task<FlightModel?> GetByIdAsync(int id)
    {
        return await _flightRepository.GetByIdAsync(id);
    }

    public async Task<int> CreateAsync(string flightNumber, string destination, DateTime departureTime, DateTime arrivalTime, int availableSeats, string airplanePhotoUrl, int airportId)
    {
        return await _flightRepository.CreateAsync(flightNumber, destination, departureTime, arrivalTime, availableSeats, airplanePhotoUrl, airportId);
    }

    public async Task<bool> UpdateAsync(int id, string flightNumber, string destination, DateTime departureTime, DateTime arrivalTime, int availableSeats, string airplanePhotoUrl, int airportId)
    {
        return await _flightRepository.UpdateAsync(id, flightNumber, destination, departureTime, arrivalTime, availableSeats, airplanePhotoUrl, airportId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _flightRepository.DeleteAsync(id);
    }
}

