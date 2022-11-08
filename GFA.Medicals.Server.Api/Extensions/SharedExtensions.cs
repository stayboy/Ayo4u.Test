using System.Linq.Expressions;

namespace GFA.Medicals.Server.Api.Extensions;

internal static class SharedExtensions
{
    public static T ToApiBaseEntity<T, U>(this T data, U updated) where T : ApiBaseEntity<int> where U : BaseEntity<int>
    {
        data.Id = updated.Id;

        data.Created = updated.Created;
        data.IsDeleted = updated.IsDeleted;
        data.CreatedByUserFullName = updated.CreatedByUserFullName;
        data.CreatedByUserEmail = updated.CreatedByUserEmail;

        return data;
    }

    public static T ToApiUUIDBaseEntity<T, U>(this T data, U updated) where T : ApiBaseEntity<Guid> where U : BaseEntity<Guid>
    {
        data.Id = updated.Id;

        data.Created = updated.Created;
        data.IsDeleted = updated.IsDeleted;
        data.CreatedByUserFullName = updated.CreatedByUserFullName;
        data.CreatedByUserEmail = updated.CreatedByUserEmail;

        return data;
    }

    public static T ToServiceUUIDBaseEntity<T, U>(this T data, U updated) where T : BaseEntity<Guid> where U : ApiBaseEntity<Guid>
    {
        data.Id = updated.Id;

        data.Created = updated.Created;
        data.IsDeleted = updated.IsDeleted;
        data.CreatedByUserFullName = updated.CreatedByUserFullName;
        data.CreatedByUserEmail = updated.CreatedByUserEmail;

        return data;
    }

    public static async Task<ServiceGFAUser?> GetGFAUser(this HttpRequest request)
    {
        var userRepository = request.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

        var email = request.HttpContext.User.Claims.First(x => x.Type == "Email").Value;

        if (!string.IsNullOrWhiteSpace(email))
        {
            return await userRepository.GetAsync(email);
        }

        return null;
    }

    public static async Task<Results<Ok<IEnumerable<T>>, NotFound, BadRequest<EntityResult<T>>>> FindResults<T, S>(Func<Task<IEnumerable<S>>> action, Expression<Func<IEnumerable<S>, IEnumerable<T>>> apiconvert, ILogger logger)
    {
        try
        {
            if (await action.Invoke() is var results && results?.Any() == true)
            {
                return TypedResults.Ok(apiconvert.Compile().Invoke(results));
            }

            return TypedResults.NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.StackTrace);

            return TypedResults.BadRequest(EntityResult.Failure<T>(ex));
        }
    }

    public static async Task<Results<Ok<T>, NotFound, BadRequest<EntityResult<T>>>> FindResult<T, S>(Func<Task<S>> action, Expression<Func<S, T>> apiconvert, ILogger logger)
    {
        try
        {
            if (await action.Invoke() is var result && (result != null || !default(S)!.Equals(result)))
            {
                return TypedResults.Ok(apiconvert.Compile().Invoke(result));
            }

            return TypedResults.NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.StackTrace);

            return TypedResults.BadRequest(EntityResult.Failure<T>(ex));
        }
    }

    public static async Task<Results<Ok<T>, UnauthorizedHttpResult, BadRequest, BadRequest<EntityResult<T>>, Created<T>>> ProcessCreatedInt<T, S>(this HttpRequest request, Func<ServiceGFAUser, Task<EntityResult<S>>> action,
        Expression<Func<S, T>> apiconvert, ILogger logger, bool isCreated = true) where T : ApiBaseEntity where S : BaseEntity<int>
    {
        try
        {
            var gfaUser = await request.GetGFAUser();
            if (gfaUser == null) return TypedResults.Unauthorized();

            var rs = await action.Invoke(gfaUser);

            if (rs.IsSuccessful && rs.Result != null)
            {
                if (isCreated == false) return TypedResults.Ok(apiconvert.Compile().Invoke(rs.Result));

                return TypedResults.Created($"/{rs.Result.Id}", apiconvert.Compile().Invoke(rs.Result));
            }

            return TypedResults.BadRequest(EntityResult.Failure<T>(rs.Errors!));
        }
        catch (Exception ex)
        {
            logger.LogError(ex.StackTrace);

            return TypedResults.BadRequest(EntityResult.Failure<T>(ex));
        }
    }

    public static async Task<Results<Ok<T>, UnauthorizedHttpResult, BadRequest<EntityResult<T>>>> ProcessResult<T, S>(this HttpRequest request, Func<ServiceGFAUser, Task<EntityResult<S>>> action,
        Expression<Func<S, T>> apiconvert, ILogger logger) where T : ApiBaseEntity
    {
        try
        {
            var gfaUser = await request.GetGFAUser();
            if (gfaUser == null) return TypedResults.Unauthorized();

            var rs = await action.Invoke(gfaUser);

            if (rs.IsSuccessful && rs.Result != null)
            {
                return TypedResults.Ok(apiconvert.Compile().Invoke(rs.Result));
            }

            return TypedResults.BadRequest(EntityResult.Failure<T>(rs.Errors!));
        }
        catch (Exception ex)
        {
            logger.LogError(ex.StackTrace);

            return TypedResults.BadRequest(EntityResult.Failure<T>(ex));
        }
    }

    public static async Task<Results<Ok<T>, UnauthorizedHttpResult, BadRequest<EntityResult<T>>>> ProcessResult<T, S>(this HttpRequest request, Func<ServiceGFAUser, Task<EnumerableEntityResult<S>>> action,
        Expression<Func<S, T>> apiconvert, ILogger logger) where T : ApiBaseEntity
    {
        try
        {
            var gfaUser = await request.GetGFAUser();
            if (gfaUser == null) return TypedResults.Unauthorized();

            var rs = await action.Invoke(gfaUser);

            if (rs.IsSuccessful && rs.Results?.Any() == true)
            {
                return TypedResults.Ok(apiconvert.Compile().Invoke(rs.Results.ElementAt(0)));
            }

            return TypedResults.BadRequest(EntityResult.Failure<T>(rs.Errors!));
        }
        catch (Exception ex)
        {
            logger.LogError(ex.StackTrace);

            return TypedResults.BadRequest(EntityResult.Failure<T>(ex));
        }
    }

    public static async Task<Results<Ok<IEnumerable<T>>, UnauthorizedHttpResult, BadRequest<EntityResult<T>>>> ProcessResults<T, S>(this HttpRequest request, Func<ServiceGFAUser, Task<EnumerableEntityResult<S>>> action,
        Expression<Func<IEnumerable<S>, IEnumerable<T>>> apiconvert, ILogger logger) where T : ApiBaseEntity
    {
        try
        {
            var gfaUser = await request.GetGFAUser();
            if (gfaUser == null) return TypedResults.Unauthorized();

            var rs = await action.Invoke(gfaUser);

            if (rs.IsSuccessful && rs.Results?.Any() == true)
            {
                return TypedResults.Ok(apiconvert.Compile().Invoke(rs.Results));
            }

            return TypedResults.BadRequest(EntityResult.Failure<T>(rs.Errors!));
        }
        catch (Exception ex)
        {
            logger.LogError(ex.StackTrace);

            return TypedResults.BadRequest(EntityResult.Failure<T>(ex));
        }
    }
}
