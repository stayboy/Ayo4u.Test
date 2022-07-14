namespace Ayo4u.Data.Extensions;

internal static class UserExtensions
{
    public static string ToDataAyoUserFullName(this AyoUser user) =>
        $"{user.FirstName} {user.LastName}";

    public static AyoUser ToAyoUser (this ServiceAyoUser user)
    {
        return new AyoUser()
        {
            Id = user.Id,

            CreatedAt = user.Created,
            IsDeleted = user.IsDeleted,

            Email = user.EmailAddress,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    public static ServiceAyoUser ToServiceAyoUser(this AyoUser user)
    {
        var result = new ServiceAyoUser()
        {
            EmailAddress = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,

            FullName = user.ToDataAyoUserFullName()
        };

        return user.ToUUIDBaseEntity(result);
    }

    public static IEnumerable<ServiceAyoUser> ToServiceAyoUsers(this IEnumerable<AyoUser> users) =>
       users.Select(x => x.ToServiceAyoUser());

    public static AyoUser ToUserUpdate(this DataAyoUserUpdate value, AyoUser? user, AyoUser? updateModel = null)
    {
        updateModel ??= new()
        {
            Id = value.Id,
            CreatedAt = value.CreatedAt,
            CreatedByUserId = user?.Id
        };
        
        if (user != null)
        {
            updateModel.ModifiedByUserId = user?.Id;
        }

        updateModel.Email = value.Email;
        updateModel.FirstName = value.FirstName;
        updateModel.LastName = value.LastName;

        return updateModel;
        
    }
}
