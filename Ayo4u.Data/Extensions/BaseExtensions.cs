using Ayo4u.Data.Models;
using Ayo4u.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayo4u.Data.Extensions;

internal static class BaseExtensions
{
    public static U ToBaseEntity<T, U>(this T data, U updated) where T : IdModel<int, T> where U : BaseEntity<int>
    {
        updated.Id = data.Id;

        updated.Created = data.CreatedAt;
        updated.IsDeleted = data.IsDeleted;
        updated.CreatedByUserFullName = data.CreatedByUser?.ToDataAyoUserFullName();
        updated.CreatedByUserEmail = data.CreatedByUser?.Email;

        return updated;
    }

    public static U ToUUIDBaseEntity<T, U>(this T data, U updated) where T : IdModel<Guid, T> where U : BaseEntity<Guid>
    {
        updated.Id = data.Id;

        updated.Created = data.CreatedAt;
        updated.IsDeleted = data.IsDeleted;
        updated.CreatedByUserFullName = data.CreatedByUser?.ToDataAyoUserFullName();
        updated.CreatedByUserEmail = data.CreatedByUser?.Email;

        return updated;
    }
}
