using Server.Shared.Constants;
using Server.Shared.Services;

namespace GFA.Medicals.Server.Api.EndPoints;

internal static class CodeApi
{
    private static readonly ILogger? logger;
    public static RouteGroupBuilder MapConversionsApi(this RouteGroupBuilder group, ILogger logger)
    {
        logger = logger ?? throw new ArgumentNullException(nameof(logger));

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

        group.MapPost("/{id}/block", BlockCode);

        group.MapPost("/{id}/activate", UnBlockCode);

        group.MapPost("/{id}/clone", CloneCode);

        return group.RequireAuthorization();
    }

    static Task<Results<Ok<ApiValueCodeType>, NotFound, BadRequest<EntityResult<ApiValueCodeType>>>> GetCodeById(int id, ICodeTypeRepository codeRepository) =>
        SharedExtensions.FindResult(() => codeRepository.GetAsync(id), q => q!.ToApiValueCodeType(), logger!);

    static Task<Results<Ok<IEnumerable<ApiValueCodeType>>, NotFound, BadRequest<EntityResult<ApiValueCodeType>>>> FindCodes(ApiValueCodeTypeSearchParameters parameters, ICodeTypeRepository codeRepository) =>
        SharedExtensions.FindResults(() => codeRepository.BrowseAsync(parameters.ToValueCodeTypeSearchParameters()), q => q.ToApiValueCodeTypes(), logger!);

    static Task<Results<Ok<ApiValueCodeType>, UnauthorizedHttpResult, BadRequest, BadRequest<EntityResult<ApiValueCodeType>>, Created<ApiValueCodeType>>> AddUpdateCode(ApiValueCodeTypeChange change, IClock clock,
        ICodeTypeRepository codeRepository, HttpRequest request) =>
        request.ProcessCreatedInt(gfaUser => codeRepository.AddUpdateEntity(change.ToDataValueCodeTypeUpdate(clock.Now(), gfaUser)), q => q.ToApiValueCodeType(), logger!, change.Id == 0);
           
    static Task<Results<Ok<ApiValueCodeType>, UnauthorizedHttpResult, BadRequest<EntityResult<ApiValueCodeType>>>> DeleteCode(int id, ICodeTypeRepository codeRepository, HttpRequest request) =>
        request.ProcessResult(gfaUser => codeRepository.SetStatus(new[] { id }, BlockStatus.Deleted, gfaUser), q => q.ToApiValueCodeType(), logger!);

    static Task<Results<Ok<ApiValueCodeType>, UnauthorizedHttpResult, BadRequest<EntityResult<ApiValueCodeType>>>> BlockCode(int id, ICodeTypeRepository codeRepository, HttpRequest request) =>
        request.ProcessResult(gfaUser => codeRepository.SetStatus(new[] { id }, BlockStatus.Blocked, gfaUser), q => q.ToApiValueCodeType(), logger!);

    static Task<Results<Ok<ApiValueCodeType>, UnauthorizedHttpResult, BadRequest<EntityResult<ApiValueCodeType>>>> UnBlockCode(int id, ICodeTypeRepository codeRepository, HttpRequest request) =>
        request.ProcessResult(gfaUser => codeRepository.SetStatus(new[] { id }, BlockStatus.Activate, gfaUser), q => q.ToApiValueCodeType(), logger!);

    static Task<Results<Ok<ApiValueCodeType>, UnauthorizedHttpResult, BadRequest<EntityResult<ApiValueCodeType>>>> CloneCode(ApiValueCodeTypeChange change, IClock clock, ICodeTypeRepository codeRepository, HttpRequest request) =>
        request.ProcessResult((gfaUser) =>
            {
                var updated = change.ToDataValueCodeTypeUpdate(clock.Now(), gfaUser);

                return codeRepository.CloneAsync(updated);

            }, q => q.ToApiValueCodeType(), logger!);
}
