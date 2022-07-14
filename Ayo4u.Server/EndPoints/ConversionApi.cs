using Ayo4u.Infrastructure.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Ayo4u.Server.EndPoints;

internal static class ConversionApi
{
    public static GroupRouteBuilder MapConversionsApi(this GroupRouteBuilder group)
    {
        group.MapGet("/{id}", GetConversionById);

        group.MapPost("/search", FindConversions);

        group.MapPut("/{id}", UpdateConversion);

        group.MapPost("/", AddUpdateConversion);

        group.MapDelete("/{id}", DeleteConversion);

        group.MapPost("/{id:guid}/block", BlockConversion);

        group.MapPost("/{id}/activate", UnBlockConversion);

        group.MapGet("/{input}-{output}/convert/{value}", ConvertValue);

        group.MapGet("/{id}/convert/{value}", ConvertValueById);

        return group;
    }

    static async Task<Results<Ok<ServiceRequestAction>, BadRequest>> ConvertValue(string input, string output, float value, IClock clock, IConverterRepository converterRepository,
        IRequestActionRepository requestRepository)
    {
        var request = new ApiConversionInput(value, input, output);

        var change = await request.ToApiRequestChange(clock.Now(), async (inType, outType) =>
        {
            return (await converterRepository.GetAsync(inType, outType)).ToApiUnitConverter();
        });

        if (change != null)
        {
            var rs = await requestRepository.AddUpdateRequestAction(change.ToDataRequestActionChange(clock.Now()));

            if (rs.IsSuccessful && rs.Result != null)
            {
                return TypedResults.Ok(rs.Result);
            }
        }

        return TypedResults.BadRequest();
    }

    public static async Task<IResult> ConvertValueById(int id, float value, IClock clock, IConverterRepository converterRepository,
        IRequestActionRepository requestRepository)
    {
        var request = new ApiUnitConversionInput(value, id);

        var change = await request.ToApiUnitRequestChange(clock.Now(), async (convertId) =>
        {
            return (await converterRepository.GetAsync(convertId)).ToApiUnitConverter();
        });

        if (change != null)
        {
            var rs = await requestRepository.AddUpdateRequestAction(change.ToDataRequestActionChange(clock.Now()));

            if (rs.IsSuccessful && rs.Result != null)
            {
                return Results.Ok(rs.Result);
            }
        }

        return Results.BadRequest();
    }

    public static async Task<IResult> UnBlockConversion(int id, IConverterRepository convertRepository) =>
        await SetStatus(id, BlockStatus.Activate, convertRepository) is ApiUnitConverter converter ? Results.Ok(converter) : Results.BadRequest();

    public static async Task<IResult> BlockConversion(int id, IConverterRepository convertRepository) =>
        await SetStatus(id, BlockStatus.Blocked, convertRepository) is ApiUnitConverter converter ? Results.Ok(converter) : Results.BadRequest();

    public static async Task<IResult> DeleteConversion(int id, IConverterRepository convertRepository) =>
        await SetStatus(id, BlockStatus.Deleted, convertRepository) is ApiUnitConverter converter ? Results.Ok(converter) : Results.BadRequest();

    public static async Task<IResult> CreateConversion(ApiUnitConverterChange change, IClock clock, IConverterRepository converterRepository)
    {
        var rs = await AddUpdateConversion(change, clock.Now(), converterRepository);

        if (rs != null)
        {
            return Results.CreatedAtRoute($"/{rs.Id}");
        }

        return Results.BadRequest();
    }

    public static async Task<IResult> UpdateConversion(int id, ApiUnitConverterChange change, IClock clock, IConverterRepository convertRepository)
    {
        if ((await convertRepository.GetAsync(id))?.ToApiUnitConverter() is ApiUnitConverter converter)
        {
            var rs = await AddUpdateConversion(change, clock.Now(), convertRepository);

            if (rs == null) return Results.BadRequest();

            return Results.NoContent();
        }

        return Results.NotFound();
    }

    public static async Task<IResult> GetConversionById(int id, IConverterRepository convertRepository) =>
        (await convertRepository.GetAsync(id))?.ToApiUnitConverter() is ApiUnitConverter converter ? Results.Ok(converter) : Results.NotFound();

    public static async Task<IResult> FindConversions(ApiConverterSearchParameters parameters, IConverterRepository convertRepository)
    {
        return Results.Ok((await convertRepository.BrowseAsync(parameters.ToConversionSearchParameters())).ToApiUnitConverters());
    }

    private static async Task<ApiUnitConverter?> AddUpdateConversion(ApiUnitConverterChange change, DateTime now, 
        IConverterRepository convertRepository)
    {
        var rs = await convertRepository.AddUpdateConverter(change.ToDataUnitConverterUpdate(now));

        if (rs.IsSuccessful && rs.Result != null)
        {
            return rs.Result.ToApiUnitConverter();
        }

        return default;
    }

    private static async Task<ApiUnitConverter?> SetStatus(int id, BlockStatus status, IConverterRepository convertRepository)
    {
        var rs = await convertRepository.SetStatus(id, status);

        if (rs.IsSuccessful && rs.Result != null)
        {
            return rs.Result.ToApiUnitConverter();
        }

        return default;
    }
}
