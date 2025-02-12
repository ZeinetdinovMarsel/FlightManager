using FM.API.Extentions;
using FM.Application.Services;
using FM.Core.Enums;

namespace FM.API.Endpoints;

public static class FederalDistrictEndpoints
{
    public static IEndpointRouteBuilder MapFederalDistrictEndpoints(
    this IEndpointRouteBuilder app)
    {
        app.MapGet("federalDistricts", GetFederalDistricts);
        app.MapGet("federalDistricts/{id}", GetFederalDistrictById);
        app.MapPost("federalDistricts", CreateFederalDistrict).RequireRoles(Role.Admin);
        app.MapPut("federalDistricts/{id}", UpdateFederalDistrict).RequireRoles(Role.Admin);
        app.MapDelete("federalDistricts/{id}", DeleteFederalDistrict).RequireRoles(Role.Admin);

        return app;
    }


    public static async Task<IResult> GetFederalDistricts(IFederalDistrictService service,
        string? sortBy = null, bool descending = false, int page = 1, int pageSize = 10, string? filter = null)
    {
        try
        {
            var districts = await service.GetAllFederalDistricts(sortBy, descending, page, pageSize, filter);
            return Results.Ok(districts);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }


    public static async Task<IResult> GetFederalDistrictById(int id, IFederalDistrictService service)
    {
        try
        {
            var district = await service.GetFederalDistrictById(id);
            if (district == null)
                return Results.NotFound("Федеральный округ не найден");

            return Results.Ok(district);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public static async Task<IResult> CreateFederalDistrict(string name, IFederalDistrictService service)
    {
        try
        {
            var districtId = await service.CreateFederalDistrict(name);
            return Results.Created($"/federalDistricts/{districtId}", districtId);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public static async Task<IResult> UpdateFederalDistrict(int id, string name, IFederalDistrictService service)
    {
        try
        {
            var result = await service.UpdateFederalDistrict(id, name);
            if (!result)
                return Results.NotFound("Федеральный округ не найден");

            return Results.Ok("Федеральный округ обновлён");
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    public static async Task<IResult> DeleteFederalDistrict(int id, IFederalDistrictService service)
    {
        try
        {
            var result = await service.DeleteFederalDistrict(id);
            if (!result)
                return Results.NotFound("Федеральный округ не найден");

            return Results.Ok("Федеральный округ удалён");
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}
