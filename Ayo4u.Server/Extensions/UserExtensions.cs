using Ayo4u.Infrastructure.Models;
using Ayo4u.Infrastructure.Queries;

namespace Ayo4u.Server.Api.Extensions
{
    internal static class UserExtensions
    {
        public static DataAyoUserUpdate ToDataAyoUserUpdate(this ApiAyoUserChange change, DateTime now, ServiceAyoUser? user = null)
        {
            return new(change.Id, change.Created ?? now)
            {
                Email = change.Email,
                FirstName = change.FirstName,
                LastName = change.LastName,
                AyoUser = user
            };
        }

        public static IEnumerable<ApiAyoUser> ToApiAyoUsers(this IEnumerable<ServiceAyoUser> users) =>
            users.Select(x => x.ToApiAyoUser());

        public static ApiAyoUser ToApiAyoUser(this ServiceAyoUser user)
        {
            var result = new ApiAyoUser()
            {
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserFullName = user.FullName
            };

            return result.ToApiUUIDBaseEntity(user);
        }

        public static ServiceAyoUser ToServiceAyoUser(this ApiAyoUser user)
        {
            var result = new ServiceAyoUser()
            {
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,

                FullName = user.UserFullName
            };

            return result.ToServiceUUIDBaseEntity(user);
        }

        public static UserSearchParameters ToUserSearchParameters(this ApiUserSearchParameters parameters)
        {
            return new()
            {
                Email = parameters.email,
                EndCreatedAt = parameters.toCreatedDate,
                IsDeleted = parameters.deleted,
                StartCreatedAt = parameters.fromCreatedDate,
                UserIds = parameters.UserIds,
                UserName = parameters.name
            };
        }


    }
}
