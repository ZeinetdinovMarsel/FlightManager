using FM.API.Contracts;
using FM.API.Endpoints;
using FM.Application.Services;
using FM.Core.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace FM.API.Tests;

public class AirportEndpointsTests
{
    private readonly Mock<IAirportService> _airportServiceMock = new();

    [Fact]
    public async Task GetAirports_ShouldReturnOk_WhenAirportsExist()
    {
        var airports = new List<AirportModel>
        {
            AirportModel.Create(1, "Airport 1", "City 1", 1),
            AirportModel.Create(2, "Airport 2", "City 2", 2)
        };

        _airportServiceMock.Setup(service => service.GetAllAsync(
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            null))
            .ReturnsAsync(airports);

        var result = await AirportEndpoints.GetAirports(_airportServiceMock.Object);

        var okResult = Assert.IsType<Ok<IEnumerable<AirportModel>>>(result);
        Assert.Equal(2, okResult.Value?.Count());
    }

    [Fact]
    public async Task GetAirports_ShouldCallServiceWithCorrectParameters()
    {
        const string sortBy = "Name";
        const bool descending = true;
        const int page = 2;
        const int pageSize = 20;
        const string cityFilter = "test";
        const string nameFilter = "test";
        const int federalDistrictFilter = 1;

        await AirportEndpoints.GetAirports(
            _airportServiceMock.Object,
            sortBy,
            descending,
            page,
            pageSize,
            cityFilter,
            nameFilter,
            federalDistrictFilter);

        _airportServiceMock.Verify(x => x.GetAllAsync(
            sortBy,
            descending,
            page,
            pageSize,
            cityFilter,
            nameFilter,
            federalDistrictFilter),
            Times.Once);
    }

    [Fact]
    public async Task GetAirportById_ShouldReturnAirport_WhenExists()
    {

        const int id = 1;
        var expectedAirport = AirportModel.Create(id, "Test", "City", 1);
        _airportServiceMock.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(expectedAirport);

        var result = await AirportEndpoints.GetAirportById(id, _airportServiceMock.Object);

        var okResult = Assert.IsType<Ok<AirportModel>>(result);
        Assert.Equal(expectedAirport, okResult.Value);
        _airportServiceMock.Verify(x => x.GetByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetAirportById_ShouldReturnNotFound_WhenNotExists()
    {
        const int id = 999;
        _airportServiceMock.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((AirportModel)null!);

        var result = await AirportEndpoints.GetAirportById(id, _airportServiceMock.Object);

        var notFoundResult = Assert.IsType<NotFound<string>>(result);
        Assert.Equal("Аэропорт не найден", notFoundResult.Value);
    }

    [Fact]
    public async Task CreateAirport_ShouldReturnCreatedWithId_WhenSuccessful()
    {
        var request = new AirportRequest
        {
            Name = "New",
            City = "City",
            FederalDistrictId = 1
        };
        const int expectedId = 123;

        _airportServiceMock.Setup(x => x.CreateAsync(
            request.Name,
            request.City,
            request.FederalDistrictId))
            .ReturnsAsync(expectedId);

        var result = await AirportEndpoints.CreateAirport(request, _airportServiceMock.Object);

        var createdResult = Assert.IsType<Created<int>>(result);
        Assert.Equal($"/airports/{expectedId}", createdResult.Location);
        Assert.Equal(expectedId, createdResult.Value);

        _airportServiceMock.Verify(x => x.CreateAsync(
            request.Name,
            request.City,
            request.FederalDistrictId),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAirport_ShouldReturnOk_WhenUpdateSuccessful()
    {
        const int id = 1;
        var request = new AirportRequest
        {
            Name = "Updated",
            City = "City",
            FederalDistrictId = 2
        };

        _airportServiceMock.Setup(x => x.UpdateAsync(
            id,
            request.Name,
            request.City,
            request.FederalDistrictId))
            .ReturnsAsync(true);

        var result = await AirportEndpoints.UpdateAirport(id, request, _airportServiceMock.Object);

        var okResult = Assert.IsType<Ok<string>>(result);
        Assert.Equal("Аэропорт обновлён", okResult.Value);

        _airportServiceMock.Verify(x => x.UpdateAsync(
            id,
            request.Name,
            request.City,
            request.FederalDistrictId),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAirport_ShouldReturnNotFound_WhenAirportNotExists()
    {
        const int id = 999;
        var request = new AirportRequest
        {
            Name = "Updated",
            City = "City",
            FederalDistrictId = 2
        };

        _airportServiceMock.Setup(x => x.UpdateAsync(
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<int>()))
            .ReturnsAsync(false);

        var result = await AirportEndpoints.UpdateAirport(id, request, _airportServiceMock.Object);

        var notFoundResult = Assert.IsType<NotFound<string>>(result);
        Assert.Equal("Аэропорт не найден", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteAirport_ShouldReturnOk_WhenDeleteSuccessful()
    {
        const int id = 1;
        _airportServiceMock.Setup(x => x.DeleteAsync(id))
            .ReturnsAsync(true);

        var result = await AirportEndpoints.DeleteAirport(id, _airportServiceMock.Object);

        var okResult = Assert.IsType<Ok<string>>(result);
        Assert.Equal("Аэропорт удалён", okResult.Value);
        _airportServiceMock.Verify(x => x.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task DeleteAirport_ShouldReturnNotFound_WhenAirportNotExists()
    {
        const int id = 999;
        _airportServiceMock.Setup(x => x.DeleteAsync(id))
            .ReturnsAsync(false);

        var result = await AirportEndpoints.DeleteAirport(id, _airportServiceMock.Object);

        var notFoundResult = Assert.IsType<NotFound<string>>(result);
        Assert.Equal("Аэропорт не найден", notFoundResult.Value);
    }

    [Fact]
    public async Task AnyEndpoint_ShouldReturnProblem_WhenServiceThrowsException()
    {
        const string errorMessage = "Test exception";
        _airportServiceMock.Setup(x => x.GetAllAsync(
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            null))
            .ThrowsAsync(new Exception(errorMessage));

        var result = await AirportEndpoints.GetAirports(_airportServiceMock.Object);

        var problemResult = Assert.IsType<ProblemHttpResult>(result);
        Assert.NotNull(problemResult.ProblemDetails.Detail);
        Assert.Equal(errorMessage, problemResult.ProblemDetails.Detail);
    }
}