using System.Linq.Expressions;
using System.Reflection;

namespace GFA.Medicals.Data.Extensions;

internal static class BaseExtensions
{
    public static IQueryable<T> ApplySortingFields<T>(this IQueryable<T> query, SortField[] sortFields)
    {
        IOrderedQueryable<T> ordered;

        var field_1 = sortFields.ElementAt(0);
        var pInfo = typeof(T).GetProperty(field_1.Name, BindingFlags.Public | BindingFlags.Static);

        if (pInfo != null)
        {
            ordered = field_1.IsAscending switch
            {
                false => query.OrderByDescending(GetSortExpression<T>(pInfo)),
                _ => query.OrderBy(GetSortExpression<T>(pInfo))
            };

            if (sortFields.Skip(1) is var sfields && sfields?.Any() == true)
            {
                foreach (var field in sfields)
                {
                    var propInfo = typeof(T).GetProperty(field.Name, BindingFlags.Public | BindingFlags.Static);

                    if (propInfo != null)
                    {
                        query = field.IsAscending switch
                        {
                            false => ordered.ThenByDescending(GetSortExpression<T>(propInfo)),
                            _ => ordered.ThenBy(GetSortExpression<T>(propInfo))
                        };
                    }
                }
            }
            return ordered;
        }

        return query;
    }

    private static Expression<Func<T, object>> GetSortExpression<T>(PropertyInfo mProp)
    {
        var prm = Expression.Parameter(typeof(T), "x");

        var property = Expression.PropertyOrField(prm, mProp.Name);

        return Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), prm);
    }

    internal static T? CloneBase<T, B>(T obj, Guid userId, Func<T, T> updateHandler, B initValue) where T: GFAIdModel<B, T> where B: struct
    {
        var nw = obj.Clone();

        if (nw != null)
        {
            nw.Id = initValue;
            nw.PreviousId = obj.Id;

            nw = updateHandler.Invoke(nw);

            obj.IsDeleted = true;
            obj.ModifiedByUserId = userId;
        }

        return nw;
    }

    public static T? CloneModelInt<T>(this T obj, Guid userId, Func<T, T> updateHandler) where T: GFAIdModel<int, T>
    {
        return CloneBase(obj, userId, updateHandler, 0);
    }

    public static T? CloneModelGUID<T>(this T obj, Guid userId, Func<T, T> updateHandler) where T : GFAIdModel<Guid, T>
    {
        return CloneBase(obj, userId, updateHandler, Guid.Empty);
    }

    public static T? Clone<T>(this T obj) where T : class
    {
        var inst = obj!.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);

        if (inst != null && obj != null)
        {
            return (T?)inst.Invoke(obj, null);
        }

        return default(T);
    }

    public static U ToBaseEntity<T, U>(this T data, U updated) where T : GFAIdModel<int, T> where U : BaseEntity<int>
    {
        updated.Id = data.Id;

        updated.Created = data.CreatedAt;
        updated.IsDeleted = data.IsDeleted;
        updated.CreatedByUserFullName = data.CreatedByUser?.ToDataGFAUserFullName();
        updated.CreatedByUserEmail = data.CreatedByUser?.Email;

        return updated;
    }

    public static U ToUUIDBaseEntity<T, U>(this T data, U updated) where T : GFAIdModel<Guid, T> where U : BaseEntity<Guid>
    {
        updated.Id = data.Id;

        updated.Created = data.CreatedAt;
        updated.IsDeleted = data.IsDeleted;
        updated.CreatedByUserFullName = data.CreatedByUser?.ToDataGFAUserFullName();
        updated.CreatedByUserEmail = data.CreatedByUser?.Email;

        return updated;
    }
}
