using FM.API.Contracts;
using FM.API.Extentions;
using FM.Application.Services;
using FM.Core.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace FM.API.Endpoints;

public static class ServiceEndpoints
{
    public static IEndpointRouteBuilder MapServiceEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapGet("services", GetServices);
        app.MapGet("services/{id}", GetServiceById);
        app.MapPost("services", CreateService).RequireRoles(Role.Admin);
        app.MapPut("services/{id}", UpdateService).RequireRoles(Role.Admin);
        app.MapDelete("services/{id}", DeleteService).RequireRoles(Role.Admin);

        return app;
    }

    public static async Task<IResult> GetServices(IServiceService service,
        string? sortBy = null, bool descending = false, int page = 1, int pageSize = 10,
        string? nameFilter = null, decimal? costFilter = null)
    {
        try
        {
            var services = await service.GetAllAsync(sortBy, descending, page, pageSize, nameFilter, costFilter);
            return Results.Ok(services);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public static async Task<IResult> GetServiceById(int id, IServiceService service)
    {
        try
        {
            var serviceModel = await service.GetByIdAsync(id);
            return serviceModel != null
                ? Results.Ok(serviceModel)
                : Results.NotFound("Услуга не найдена");
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public static async Task<IResult> CreateService(ServiceRequest request, IServiceService service)
    {
        try
        {
            if (request == null)
            {
                return Results.BadRequest("Запрос не может быть пустым");
            }

            var serviceId = await service.CreateAsync(request.Name, request.Cost);
            return Results.Created($"/services/{serviceId}", serviceId);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> UpdateService(int id, ServiceRequest request, IServiceService service)
    {
        try
        {
            if (request == null)
            {
                return Results.BadRequest("Запрос не может быть пустым");
            }

            var result = await service.UpdateAsync(id, request.Name, request.Cost);
            return result
                ? Results.Ok("Услуга обновлена")
                : Results.NotFound("Услуга не найдена");
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> DeleteService(int id, IServiceService service)
    {
        try
        {
            var result = await service.DeleteAsync(id);
            return result
                ? Results.Ok("Услуга удалена")
                : Results.NotFound("Услуга не найдена");
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}