﻿using FM.Core.Models;

namespace FM.Application.Services;
public interface IFlightService
{
    Task<int> CreateAsync(string flightNumber, string destination, DateTime departureTime, DateTime arrivalTime, int availableSeats, string airplanePhotoUrl, int airportId);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<FlightModel>> GetAllAsync(string? sortBy = null, bool descending = false, int page = 1, int pageSize = 10,
        string? flightNumberFilter = null,
        string? destinationFilter = null,
        DateTime? departureTimeFilter = null,
        DateTime? arrivalTimeFilter = null,
        int? availableSeatsFilter = null,
        int? airportIdFilter = null
        );
    Task<FlightModel?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, string flightNumber, string destination, DateTime departureTime, DateTime arrivalTime, int availableSeats, string airplanePhotoUrl, int airportId);
}
