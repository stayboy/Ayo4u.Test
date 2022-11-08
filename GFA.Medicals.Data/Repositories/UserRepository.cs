namespace GFA.Medicals.Data.Repositories;

internal class UserRepository : DBRepositoryBase<GFAUser, GFADbContext>, IUserRepository
{
    private readonly ILogger<UserRepository> logger;

    public UserRepository(GFADbContext _context, ILogger<UserRepository> logger) : base(_context)
    {
        this.logger = logger;
    }

    public async Task<EntityResult<ServiceGFAUser>> AddUpdateEntity(DataGFAUserUpdate entity)
    {
        try
        {
            GFAUser? updated = null;

            if (entity.EntityId != Guid.Empty && await Entity.Include(q => q.CreatedByUser).SingleOrDefaultAsync(q => q.Id == entity.EntityId) is GFAUser value)
                updated = value;

            updated = entity.ToUserUpdate(entity.GFAUser?.ToGFAUser(), updated);

            if (updated.Id == Guid.Empty) Create(updated);

            await SaveChangesAsync();

            return EntityResult.Success(updated.ToServiceGFAUser());
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, nameof(AddUpdateEntity), nameof(UserRepository));

            return EntityResult.Failure<ServiceGFAUser>(new[] { ex.Message });
        }
    }

    public async Task<IEnumerable<ServiceGFAUser>> BrowseAsync(GFAUserSearchParameters parameters)
    {
        IQueryable<GFAUser>? query = null;

        if (parameters.Ids?.Any() ?? false)
        {
            query = FindByCondition(q => parameters.Ids.Contains(q.Id)).Include(q => q.CreatedByUser);
        }

        query ??= FindByCondition(q =>
            (string.IsNullOrWhiteSpace(parameters.SearchText) || string.IsNullOrWhiteSpace(q.Keywords) || EF.Functions.FreeText(q.Keywords, parameters.ToFreeText()!)) &&
            (string.IsNullOrWhiteSpace(parameters.LastName) && q.LastName == parameters.LastName) &&
            (string.IsNullOrWhiteSpace(parameters.Email) || q.Email == parameters.Email) &&
            q.IsDeleted == parameters.IsDeleted, Top: parameters.Top
        ).Include(q => q.CreatedByUser);

        if (query == null) return Enumerable.Empty<ServiceGFAUser>();

        if (parameters.SortFields?.Any() == true)
        {
            query = query.ApplySortingFields(parameters.SortFields);
        }

        return (await query.ToArrayAsync()).ToServiceGFAUsers();
    }

    public async Task<EntityResult<ServiceGFAUser>> CloneAsync(DataGFAUserUpdate updated)
    {
        try
        {
            if (await Entity.Where(q => q.Id == updated.EntityId).Include(q => q.CreatedByUser).SingleOrDefaultAsync() is GFAUser user)
            {
                if (updated == null)
                    throw new ArgumentNullException("Update Model cannot be null", nameof(DataGFAUserUpdate));

                var nw = user.CloneModelGUID(updated.GFAUser.Id, cloned => updated.ToUserUpdate(updated.GFAUser?.ToGFAUser(), cloned));

                if (nw != null)
                {
                    Create(nw);

                    await SaveChangesAsync();

                    return EntityResult.Success(user.ToServiceGFAUser());
                }

                logger.LogError($"Cloning model {updated} Failed", nameof(UserRepository), nameof(CloneAsync));

                return EntityResult.Failure<ServiceGFAUser>(new[] { "Cloning model failed" });
            }

            throw new ArgumentNullException("Code does not exist");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, nameof(UserRepository), nameof(CloneAsync));

            return EntityResult.Failure<ServiceGFAUser>(new[] { ex.Message });
        }
    }

    public async Task<ServiceGFAUser?> GetAsync(Guid userId)
    {
        return await FindByCondition(q => q.Id == userId).Include(q => q.CreatedByUser).SingleOrDefaultAsync() is
                GFAUser user ? user.ToServiceGFAUser() : default;
    }

    public async Task<ServiceGFAUser?> GetAsync(string email)
    {
        return await FindByCondition(q => q.Email == email && q.IsDeleted == false).Include(q => q.CreatedByUser).SingleOrDefaultAsync() is
                GFAUser user ? user.ToServiceGFAUser() : default;
    }

    public async Task<EnumerableEntityResult<ServiceGFAUser>> SetStatus(Guid[] ids, BlockStatus status, ServiceGFAUser user)
    {
        try
        {
            if (ids.Any() && await FindByCondition(q => ids.Contains(q.Id), trackChanges: true).ToArrayAsync() is GFAUser[] users && users.Any())
            {
                if (status == BlockStatus.Deleted)
                {
                    dbcontext.RemoveRange(users);
                }
                else
                {
                    foreach (var item in users)
                    {
                        if (user != null)
                        {
                            item.ModifiedByUserId = user.Id;
                        }

                        item.IsDeleted = status switch
                        {
                            BlockStatus.Activate => false,
                            BlockStatus.Blocked => true,
                            _ => item.IsDeleted
                        };
                    }
                }

                await SaveChangesAsync();

                return EntityResult.Success(users.ToServiceGFAUsers());
            }

            throw new ArgumentNullException("Ids Provided cannot be null");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, nameof(UserRepository), nameof(SetStatus));

            return EnumerableEntityResult<ServiceGFAUser>.Failure(new[] { ex.Message });
        }
    }
}
