namespace Ayo4u.Infrastructure.Repositories;

public interface IUserRepository
{
    Task<ServiceAyoUser?> Get(Guid userId);

    Task<ServiceAyoUser?> Get(string email);

    Task<EntityResult<ServiceAyoUser>> AddUpdateUser(DataAyoUserUpdate user);

    Task<IEnumerable<ServiceAyoUser>> BrowseAsync(UserSearchParameters parameters);

    Task<EntityResult<ServiceAyoUser>> SetStatus(Guid userId, BlockStatus status, DataAyoUserUpdate? user = null);
}
