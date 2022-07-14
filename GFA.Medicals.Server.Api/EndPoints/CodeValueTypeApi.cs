using Server.Shared.Constants;
using Server.Shared.Services;

namespace GFA.Medicals.Server.Api.EndPoints;

internal static class CodeValueTypeApi
{
    public static GroupRouteBuilder MapConversionsApi(this GroupRouteBuilder group)
    {
        group.MapGet("/{id}", GetCodeById);

        group.MapGet("/{codeType}/codes", (string codeType, int? top, ICodeTypeRepository rep)
            => FindCodes(new() { parentcodes = new[] { codeType }, top = top ?? 50 }, rep));

        group.MapGet("/{codeType}/codes/{code}", (string codeType, string code, int? top, ICodeTypeRepository rep)
            => FindCodes(new() { parentcodes = new[] { codeType }, childcodes = new[] { code }, top = top ?? 50 }, rep));

        group.MapGet("/{codeType}/({codes})", (string codeType, string[] codes, ICodeTypeRepository rep)
            => FindCodes(new() { parentcodes = new[] { codeType }, childcodes = codes }, rep));

        group.MapPost("/search", FindCodes);

        group.MapPost("/", AddUpdateCode);

        group.MapDelete("/{id}", DeleteCode);

        group.MapPost("/{id:guid}/block", BlockCode);

        group.MapPost("/{id}/activate", UnBlockCode);

        group.MapPost("/{id}/clone", CloneCode);

        return group;
    }

    static async Task<Results<Ok<ApiValueCodeType>, NotFound>> GetCodeById(int id, ICodeTypeRepository codeRepository) =>
        await codeRepository.GetAsync(id) is var code ? TypedResults.Ok(code!.ToApiValueCodeType()) : TypedResults.NotFound();

    static async Task<Results<Ok<IEnumerable<ApiValueCodeType>>, NotFound>> FindCodes(ApiValueCodeTypeSearchParameters parameters, ICodeTypeRepository codeRepository)
    {
        if (await codeRepository.BrowseAsync(parameters.ToValueCodeTypeSearchParameters()) is var codes)
        {
            return TypedResults.Ok(codes.ToApiValueCodeTypes());
        }

        return TypedResults.NotFound();
    }

    static async Task<Results<Ok<ApiValueCodeType>, UnauthorizedHttpResult, BadRequest, BadRequest<EntityResult<ApiValueCodeType>>, Created<ApiValueCodeType>>> AddUpdateCode(ApiValueCodeTypeChange change, IClock clock,
        ICodeTypeRepository codeRepository, HttpRequest request)
    {
        var gfaUser = await request.GetGFAUser();
        if (gfaUser == null) return TypedResults.Unauthorized();

        var rs = await codeRepository.AddUpdateEntity(change.ToDataValueCodeTypeUpdate(clock.Now(), gfaUser));

        if (rs.IsSuccessful && rs.Result != null)
        {
            if (change.Id == 0) return TypedResults.Created($"/{rs.Result.Id}", rs.Result.ToApiValueCodeType());

            return TypedResults.Ok(rs.Result.ToApiValueCodeType());
        }

        return TypedResults.BadRequest(EntityResult<ApiValueCodeType>.Failure(rs.Errors!));
    }

    static async Task<Results<Ok<ApiValueCodeType>, UnauthorizedHttpResult, BadRequest<EntityResult<ApiValueCodeType>>>> DeleteCode(int id, ICodeTypeRepository codeRepository, HttpRequest request)
    {
        var gfaUser = await request.GetGFAUser();
        if (gfaUser == null) return TypedResults.Unauthorized();

        var rs = await codeRepository.SetStatus(new[] { id }, BlockStatus.Deleted, gfaUser);

        if (rs.IsSuccessful && rs.Results != null)
        {
            return TypedResults.Ok(rs.Results.ElementAt(0).ToApiValueCodeType());
        }

        return TypedResults.BadRequest(EntityResult<ApiValueCodeType>.Failure(rs.Errors!));
    }

    static async Task<Results<Ok<ApiValueCodeType>, UnauthorizedHttpResult, BadRequest<EntityResult<ApiValueCodeType>>>> BlockCode(int id, ICodeTypeRepository codeRepository, HttpRequest request)
    {
        var gfaUser = await request.GetGFAUser();
        if (gfaUser == null) return TypedResults.Unauthorized();

        var rs = await codeRepository.SetStatus(new[] { id }, BlockStatus.Blocked, gfaUser);

        if (rs.IsSuccessful && rs.Results != null)
        {
            return TypedResults.Ok(rs.Results.ElementAt(0).ToApiValueCodeType());
        }

        return TypedResults.BadRequest(EntityResult<ApiValueCodeType>.Failure(rs.Errors!));
    }

    static async Task<Results<Ok<ApiValueCodeType>, UnauthorizedHttpResult, BadRequest<EntityResult<ApiValueCodeType>>>> UnBlockCode(int id, ICodeTypeRepository codeRepository, HttpRequest request)
    {
        var gfaUser = await request.GetGFAUser();
        if (gfaUser == null) return TypedResults.Unauthorized();

        var rs = await codeRepository.SetStatus(new[] { id }, BlockStatus.Activate, gfaUser);

        if (rs.IsSuccessful && rs.Results != null)
        {
            return TypedResults.Ok(rs.Results.ElementAt(0).ToApiValueCodeType());
        }

        return TypedResults.BadRequest(EntityResult<ApiValueCodeType>.Failure(rs.Errors!));
    }

    static async Task<Results<Ok<ApiValueCodeType>, UnauthorizedHttpResult, BadRequest<EntityResult<ApiValueCodeType>>>> CloneCode(ApiValueCodeTypeChange change, IClock clock, ICodeTypeRepository codeRepository, HttpRequest request)
    {
        var gfaUser = await request.GetGFAUser();
        if (gfaUser == null) return TypedResults.Unauthorized();

        var updated = change.ToDataValueCodeTypeUpdate(clock.Now(), gfaUser);

        var rs = await codeRepository.CloneAsync(updated);

        if (rs.IsSuccessful && rs.Result != null)
        {
            return TypedResults.Ok(rs.Result.ToApiValueCodeType());
        }

        return TypedResults.BadRequest(EntityResult<ApiValueCodeType>.Failure(rs.Errors!));
    }
}
