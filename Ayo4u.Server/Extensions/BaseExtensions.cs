using Ayo4u.Server.Shared.Models;

namespace Ayo4u.Server.Api.Extensions;

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
}
