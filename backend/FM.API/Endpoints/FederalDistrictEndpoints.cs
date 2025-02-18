using FM.API.Contracts;
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
        string? sortBy = null, bool descending = false, int page = 1, int pageSize = 10, string? namefilter = null)
    {
        try
        {
            var districts = await service.GetAllFederalDistricts(sortBy, descending, page, pageSize, namefilter);
            return Results.Ok(districts);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
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
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> CreateFederalDistrict(FederalDistrictRequest federalDistrict, IFederalDistrictService service)
    {
        try
        {
            var districtId = await service.CreateFederalDistrict(federalDistrict.Name);
            return Results.Created($"/federalDistricts/{districtId}", districtId);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public static async Task<IResult> UpdateFederalDistrict(FederalDistrictRequest federalDistrict, IFederalDistrictService service)
    {
        try
        {
            var result = await service.UpdateFederalDistrict(federalDistrict.Id, federalDistrict.Name);
            if (!result)
                return Results.NotFound("Федеральный округ не найден");

            return Results.Ok("Федеральный округ обновлён");
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
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
            return Results.BadRequest(ex.Message);
        }
    }
}
