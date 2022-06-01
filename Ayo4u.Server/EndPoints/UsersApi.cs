namespace Ayo4u.Server.EndPoints;

internal static class UsersApi
{
    public static IEndpointRouteBuilder MapUsersApi(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/users/{id:guid}", GetUserById);

        routes.MapGet("/users/{email}", GetUserByEmail);

        routes.MapPost("/users/search", FindUsers);

        routes.MapPut("/users/{id:guid}", UpdateUser);

        routes.MapPost("/users", AddUpdateUser);

        routes.MapDelete("/users/{id:guid}", DeleteUser);

        routes.MapPost("/user/{id:guid}/block", BlockUser);

        routes.MapPost("/user/{id:guid}/activate", UnBlockUser);

        return routes;
    }

    public static async Task<IResult> UnBlockUser(Guid id, IUserRepository userRepository) =>
        await SetStatus(id, BlockStatus.Activate, userRepository) is ApiAyoUser ayoUser ? Results.Ok(ayoUser) : Results.BadRequest();

    public static async Task<IResult> BlockUser(Guid id, IUserRepository userRepository) =>
        await SetStatus(id, BlockStatus.Blocked, userRepository) is ApiAyoUser ayoUser ? Results.Ok(ayoUser) : Results.BadRequest();

    public static async Task<IResult> DeleteUser(Guid id, IUserRepository userRepository) =>
        await SetStatus(id, BlockStatus.Deleted, userRepository) is ApiAyoUser ayoUser ? Results.Ok(ayoUser) : Results.BadRequest();

    public static async Task<IResult> CreateUser(ApiAyoUserChange change, IClock clock, IUserRepository userRepository)
    {        
        var rs = await AddUpdateUser(change, clock.Now(), userRepository);

        if (rs != null)
        {
            return Results.CreatedAtRoute($"/users/{rs.Id}");
        }

        return Results.BadRequest();
    }

    public static async Task<IResult> UpdateUser(Guid id, ApiAyoUserChange change, IClock clock, IUserRepository userRepository)
    {
        if ((await userRepository.Get(id))?.ToApiAyoUser() is ApiAyoUser ayoUser )
        {
            var rs = await AddUpdateUser(change, clock.Now(), userRepository);

            if (rs == null) return Results.BadRequest();

            return Results.NoContent();
        }

        return Results.NotFound();
    }

    public static async Task<IResult> GetUserByEmail(string email, IUserRepository userRepository) =>
        (await userRepository.Get(email))?.ToApiAyoUser() is ApiAyoUser user ? Results.Ok(user) : Results.NotFound();

    public static async Task<IResult> GetUserById(Guid userId, IUserRepository userRepository) =>
        (await userRepository.Get(userId))?.ToApiAyoUser() is ApiAyoUser user ? Results.Ok(user) : Results.NotFound();

    public static async Task<IResult> FindUsers(ApiUserSearchParameters parameters, IUserRepository userRepository)
    {
        return Results.Ok((await userRepository.BrowseAsync(parameters.ToUserSearchParameters())).ToApiAyoUsers());
    }

    private static async Task<ApiAyoUser?> AddUpdateUser(ApiAyoUserChange change, DateTime now, IUserRepository userRepository)
    {
        var rs = await userRepository.AddUpdateUser(change.ToDataAyoUserUpdate(now));

        if (rs.IsSuccessful && rs.Result != null)
        {
            return rs.Result.ToApiAyoUser();
        }

        return default;
    }

    private static async Task<ApiAyoUser?> SetStatus(Guid id, BlockStatus status, IUserRepository userRepository)
    {
        var rs = await userRepository.SetStatus(id, status);

        if (rs.IsSuccessful && rs.Result != null)
        {
            return rs.Result.ToApiAyoUser();
        }

        return default;
    }
}
