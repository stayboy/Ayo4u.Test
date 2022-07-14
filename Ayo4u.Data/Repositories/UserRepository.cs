using Ayo4u.Data.Extensions;
using Ayo4u.Infrastructure.Queries;
using Ayo4u.Infrastructure.Repositories;

namespace Ayo4u.Data.Repositories
{
    internal class UserRepository : DBRepositoryBase<AyoUser, AyoDbContext>, IUserRepository
    {
        public UserRepository(AyoDbContext _context) : base(_context)
        {
        }

        public async Task<EntityResult<ServiceAyoUser>> AddUpdateUser(DataAyoUserUpdate user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(DataAyoUserUpdate));

            AyoUser? updated = null;
            if (user.Id != Guid.Empty && await Entity.SingleOrDefaultAsync(q => q.Id == user.Id) is AyoUser value)
                updated = value;

            updated = user.ToUserUpdate(user.AyoUser?.ToAyoUser(), updated);

            if (user.Id == Guid.Empty) Create(updated);

            await SaveChangesAsync();

            return EntityResult<ServiceAyoUser>.Success(updated.ToServiceAyoUser());
        }

        public async Task<IEnumerable<ServiceAyoUser>> BrowseAsync(UserSearchParameters parameters)
        {
            IQueryable<AyoUser>? query = null;

            if (parameters.UserIds?.Any() ?? false)
            {
                query = FindByCondition(q => parameters.UserIds.Contains(q.Id)).Include(q => q.CreatedByUser);
            }
            else
            {
                query = FindByCondition(q =>
                    (string.IsNullOrWhiteSpace(parameters.Email) || string.Equals(q.Email, parameters.Email)) &&
                    (string.IsNullOrWhiteSpace(parameters.UserName) || (" " + q.FirstName + " " + q.LastName).Contains(parameters.UserName)) &&
                    (parameters.StartCreatedAt == null || (
                        (parameters.EndCreatedAt == null && q.CreatedAt.Date == parameters.EndCreatedAt) ||
                        (parameters.StartCreatedAt >= q.CreatedAt.Date && parameters.EndCreatedAt <= q.CreatedAt.Date)
                    )) &&
                    q.IsDeleted == parameters.IsDeleted
                ).Include(q => q.CreatedByUser);
            }

            var results = await query.ToArrayAsync();

            if (results == null) return Enumerable.Empty<ServiceAyoUser>();

            return results.ToServiceAyoUsers();
        }

        public async Task<ServiceAyoUser?> Get(Guid userId)
        {
            if (await FindByCondition(q => q.Id == userId).Include(q => q.CreatedByUser).SingleOrDefaultAsync() is AyoUser user)
            {
                return user.ToServiceAyoUser();
            }

            return default;
        }

        public async Task<ServiceAyoUser?> Get(string email)
        {
            if (await FindByCondition(q => q.Email == email).Include(q => q.CreatedByUser).SingleOrDefaultAsync() is AyoUser user)
            {
                return user.ToServiceAyoUser();
            }

            return default;
        }

        public async Task<EntityResult<ServiceAyoUser>> SetStatus(Guid userId, BlockStatus status, DataAyoUserUpdate? user)
        {
            if (await FindByCondition(q => q.Id == userId).Include(q => q.CreatedByUser).SingleOrDefaultAsync() is AyoUser result)
            {
                if (status == BlockStatus.Deleted)
                {
                    Delete(result);
                }
                else
                {
                    result.IsDeleted = status switch
                    {
                        BlockStatus.Activate => false,
                        BlockStatus.Blocked => true,
                        _ => result.IsDeleted
                    };

                    Update(result);
                }

                await SaveChangesAsync();

                return EntityResult<ServiceAyoUser>.Success(result.ToServiceAyoUser());
            }

            return EntityResult<ServiceAyoUser>.Failure(new[] { "User does not exist" });
        }
    }
}
