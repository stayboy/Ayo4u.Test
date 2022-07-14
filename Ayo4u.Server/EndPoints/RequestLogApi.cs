namespace Ayo4u.Server.EndPoints;

internal static class RequestLogApi
{
    public static IEndpointRouteBuilder MapRequestLogApi(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/logs/{id}", GetRequestLogId);

        routes.MapPost("/logs/search", FindRequestLogs);

        routes.MapDelete("/logs/collection({ids})", DeleteLogs);

        return routes;
    }

    public static async Task<IResult> GetRequestLogId(int id, IRequestActionRepository logRepository) =>
        (await logRepository.GetAsync(id))?.ToApiRequestAction() is ApiRequestAction log ? Results.Ok(log) : Results.NotFound();

    public static async Task<IResult> FindRequestLogs(ApiRequestLogSearchParameters parameters, IRequestActionRepository logRepository)
    {
        return Results.Ok((await logRepository.BrowseAsync(parameters.ToRequestActionSearchParameters())).ToApiRequestActions());
    }

    public static async Task<IResult> DeleteLogs(int[] id, IRequestActionRepository logRepository) =>
        await SetStatus(id, BlockStatus.Deleted, logRepository) is ApiRequestAction log ? Results.Ok(log) : Results.BadRequest();

    private static async Task<IEnumerable<ApiRequestAction>?> SetStatus(int[] ids, BlockStatus status, IRequestActionRepository logRepository)
    {
        var rs = await logRepository.DeleteLogs(ids, status);

        if (rs.IsSuccessful && rs.Results != null)
        {
            return rs.Results.ToApiRequestActions();
        }

        return default;
    }
}
