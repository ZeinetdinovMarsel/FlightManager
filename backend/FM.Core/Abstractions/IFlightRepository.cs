using FM.Core.Models;

namespace FM.DataAccess.Repositories
{
    public interface IFlightRepository
    {
        Task<int> CreateAsync(string flightNumber, string destination, DateTime departureTime, DateTime arrivalTime, int availableSeats, string airplanePhotoUrl, int airportId);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<FlightModel>> GetAllAsync(string? sortBy = null, bool descending = false, int page = 1, int pageSize = 10, string? filter = null);
        Task<FlightModel?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, string flightNumber, string destination, DateTime departureTime, DateTime arrivalTime, int availableSeats, string airplanePhotoUrl, int airportId);
    }
}