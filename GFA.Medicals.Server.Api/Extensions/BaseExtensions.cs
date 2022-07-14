
using Microsoft.IdentityModel.Protocols;
using System.Security.Claims;

namespace GFA.Medicals.Server.Api.Extensions;

internal static class BaseExtensions
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
}
