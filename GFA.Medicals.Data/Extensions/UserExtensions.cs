namespace GFA.Medicals.Data.Extensions;

internal static class UserExtensions
{
    public static string ToDataGFAUserFullName(this GFAUser user) =>
        $"{user.FirstName} {user.LastName}";

    public static GFAUser ToGFAUser (this ServiceGFAUser user)
    {
        return new()
        {
            Id = user.Id,

            CreatedAt = user.Created,
            IsDeleted = user.IsDeleted,

            Email = user.EmailAddress,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    public static ServiceGFAUser ToServiceGFAUser(this GFAUser user)
    {
        var result = new ServiceGFAUser()
        {
            EmailAddress = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,

            FullName = user.ToDataGFAUserFullName()
        };

        return user.ToUUIDBaseEntity(result);
    }

    public static IEnumerable<ServiceGFAUser> ToServiceGFAUsers(this IEnumerable<GFAUser> users) =>
       users.Select(x => x.ToServiceGFAUser());

    public static GFAUser ToUserUpdate(this DataGFAUserUpdate value, GFAUser? user, GFAUser? updateModel = null)
    {
        updateModel ??= new()
        {
            Id = value.EntityId,
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
