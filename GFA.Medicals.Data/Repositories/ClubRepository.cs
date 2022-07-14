namespace GFA.Medicals.Data.Repositories;

internal class ClubRepository : DBRepositoryBase<Club, GFADbContext>, IClubRepository
{
    private readonly ILogger<ClubRepository> logger;

    public ClubRepository(GFADbContext _context, ILogger<ClubRepository> logger) : base(_context)
    {
        this.logger = logger;
    }

    public async Task<EntityResult<ServiceClub>> AddUpdateEntity(DataClubUpdate entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(DataClubUpdate));

        try
        {
            Club? updated = null;

            if (entity.EntityId > 0 && await Entity.Include(q => q.CreatedByUser).SingleOrDefaultAsync(q => q.Id == entity.EntityId) is Club value)
                updated = value;

            updated = entity.ToClub(entity.GFAUser?.ToGFAUser(), updated);

            if (updated.Id == 0) Create(updated);

            await SaveChangesAsync();

            return EntityResult<ServiceClub>.Success(updated.ToServiceClub());
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, nameof(AddUpdateEntity), nameof(ClubRepository));

            return EntityResult<ServiceClub>.Failure(new[] { ex.Message });
        }
    }

    public async Task<IEnumerable<ServiceClub>> BrowseAsync(ClubSearchParameters parameters)
    {
        IQueryable<Club>? query = null;

        if (parameters.Ids?.Any() ?? false)
        {
            query = FindByCondition(q => parameters.Ids.Contains(q.Id)).Include(q => q.CreatedByUser);
        }
        else
        {
            query = FindByCondition(q =>
                (string.IsNullOrWhiteSpace(parameters.SearchText) || string.IsNullOrWhiteSpace(q.Keywords) || EF.Functions.FreeText(q.Keywords, parameters.ToFreeText()!)) &&
                (parameters.CountryId == null || q.CountryId == parameters.CountryId) &&
                (string.IsNullOrWhiteSpace(parameters.Division) || q.Division == parameters.Division) &&
                q.IsDeleted == parameters.IsDeleted, Top: parameters.Top
            ).Include(q => q.CreatedByUser);
        }

        if (query == null) return Enumerable.Empty<ServiceClub>();

        return (await query.ToArrayAsync()).ToServiceClubs();
    }

    public async Task<EntityResult<ServiceClub>> CloneAsync(DataClubUpdate updated)
    {
        try
        {
            if (await Entity.Where(q => q.Id == updated.EntityId).Include(q => q.CreatedByUser).SingleOrDefaultAsync() is Club club)
            {
                if (updated == null)
                    throw new ArgumentNullException("Update Model cannot be null", nameof(DataClubUpdate));

                var nw = club.CloneModelInt(updated.GFAUser.Id, cloned => updated.ToClub(updated.GFAUser?.ToGFAUser(), cloned));

                if (nw != null)
                {
                    Create(nw);

                    await SaveChangesAsync();

                    return EntityResult<ServiceClub>.Success(club.ToServiceClub());
                }

                logger.LogError($"Cloning model {updated} Failed", nameof(ClubRepository), nameof(CloneAsync));

                return EntityResult<ServiceClub>.Failure(new[] { "Cloning model failed" });
            }

            throw new ArgumentNullException("Code does not exist");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, nameof(ClubRepository), nameof(CloneAsync));

            return EntityResult<ServiceClub>.Failure(new[] { ex.Message });
        }
    }

    public async Task<ServiceClub?> GetAsync(int id)
    {
        return await FindByCondition(q => q.Id == id).Include(q => q.CreatedByUser).SingleOrDefaultAsync() is
        Club club ? club.ToServiceClub() : default;
    }

    public async Task<EnumerableEntityResult<ServiceClub>> SetStatus(int[] ids, BlockStatus status, ServiceGFAUser user)
    {
        try
        {
            if (ids.Any() && await FindByCondition(q => ids.Contains(q.Id), trackChanges: true).ToArrayAsync() is Club[] clubs && clubs.Any())
            {
                if (status == BlockStatus.Deleted)
                {
                    dbcontext.RemoveRange(clubs);
                }
                else
                {
                    foreach (var item in clubs)
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

                return EnumerableEntityResult<ServiceClub>.Success(clubs.ToServiceClubs());
            }

            throw new ArgumentNullException("Ids Provided cannot be null");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, nameof(ClubRepository), nameof(SetStatus));

            return EnumerableEntityResult<ServiceClub>.Failure(new[] { ex.Message });
        }
    }
}
