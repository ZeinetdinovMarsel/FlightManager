using FM.API.Contracts;
using FM.API.Endpoints;
using FM.Application.Services;
using FM.Core.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace FM.API.Tests;
public class AirportEndpointsTests
{
    private readonly Mock<IAirportService> _airportServiceMock;

    public AirportEndpointsTests()
    {
        _airportServiceMock = new Mock<IAirportService>();
    }

    [Fact]
    public async Task GetAirports_ShouldReturnOk_WhenAirportsExist()
    {
        var airports = new List<AirportModel>
            {
                AirportModel.Create(1, "Airport 1", "City 1", 1),
                AirportModel.Create(2, "Airport 2", "City 2", 2)
            };
        _airportServiceMock.Setup(service => service.GetAllAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(airports);

        var result = await AirportEndpoints.GetAirports(_airportServiceMock.Object);

        var okResult = Assert.IsType<Ok<IEnumerable<AirportModel>>>(result);
        Assert.Equal(2, okResult.Value.Count());
    }

    [Fact]
    public async Task GetAirportById_ShouldReturnOk_WhenAirportExists()
    {
        var airport = AirportModel.Create(1, "Airport 1", "City 1", 1);
        _airportServiceMock.Setup(service => service.GetByIdAsync(1))
            .ReturnsAsync(airport);

        var result = await AirportEndpoints.GetAirportById(1, _airportServiceMock.Object);

        var okResult = Assert.IsType<Ok<AirportModel>>(result); 
        Assert.Equal(1, okResult.Value.Id);
    }

    [Fact]
    public async Task GetAirportById_ShouldReturnNotFound_WhenAirportDoesNotExist()
    {
        _airportServiceMock.Setup(service => service.GetByIdAsync(1))
            .ReturnsAsync((AirportModel)null);

        var result = await AirportEndpoints.GetAirportById(1, _airportServiceMock.Object);
        
        var notFoundResult = Assert.IsType<NotFound<string>>(result); 
        Assert.Equal("Аэропорт не найден", notFoundResult.Value);
    }


    [Fact]
    public async Task CreateAirport_ShouldReturnCreated_WhenAirportIsCreated()
    {
        var request = new AirportRequest
        {
            Name = "New Airport",
            City = "New City",
            FederalDistrictId = 1
        };

        _airportServiceMock.Setup(service => service.CreateAsync(request.Name, request.City, request.FederalDistrictId))
            .ReturnsAsync(1);

        var result = await AirportEndpoints.CreateAirport(request, _airportServiceMock.Object);

        var createdResult = Assert.IsType<Created<int>>(result);
        Assert.Equal("/airports/1", createdResult.Location);
    }

    [Fact]
    public async Task UpdateAirport_ShouldReturnOk_WhenAirportIsUpdated()
    {
        var request = new AirportRequest
        {
            Name = "Updated Airport",
            City = "Updated City",
            FederalDistrictId = 1
        };

        _airportServiceMock.Setup(service => service.UpdateAsync(1, request.Name, request.City, request.FederalDistrictId))
            .ReturnsAsync(true);

        var result = await AirportEndpoints.UpdateAirport(1, request, _airportServiceMock.Object);

        var okResult = Assert.IsType<Ok<string>>(result);
        Assert.Equal("Аэропорт обновлён", okResult.Value);
    }

    [Fact]
    public async Task UpdateAirport_ShouldReturnNotFound_WhenAirportDoesNotExist()
    {
        var request = new AirportRequest
        {
            Name = "Updated Airport",
            City = "Updated City",
            FederalDistrictId = 1
        };

        _airportServiceMock.Setup(service => service.UpdateAsync(1, request.Name, request.City, request.FederalDistrictId))
            .ReturnsAsync(false);

        var result = await AirportEndpoints.UpdateAirport(1, request, _airportServiceMock.Object);

        var notFoundResult = Assert.IsType<NotFound<string>>(result);
        var returnValue = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Аэропорт не найден", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteAirport_ShouldReturnOk_WhenAirportIsDeleted()
    {
        _airportServiceMock.Setup(service => service.DeleteAsync(1))
            .ReturnsAsync(true);

        var result = await AirportEndpoints.DeleteAirport(1, _airportServiceMock.Object);

        var okResult = Assert.IsType<Ok<string>>(result);
        var returnValue = Assert.IsType<string>(okResult.Value);
        Assert.Equal("Аэропорт удалён", okResult.Value);
    }

    [Fact]
    public async Task DeleteAirport_ShouldReturnNotFound_WhenAirportDoesNotExist()
    {
        _airportServiceMock.Setup(service => service.DeleteAsync(1))
            .ReturnsAsync(false);
        var result = await AirportEndpoints.DeleteAirport(1, _airportServiceMock.Object);

        var okResult = Assert.IsType<NotFound<string>>(result);
        var returnValue = Assert.IsType<string>(okResult.Value);
        Assert.Equal("Аэропорт не найден", okResult.Value);
    }
}
