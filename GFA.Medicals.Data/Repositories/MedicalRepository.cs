namespace GFA.Medicals.Data.Repositories;

internal class MedicalRepository : DBRepositoryBase<Medical, GFADbContext>, IMedicalRepository
{
    private readonly ILogger<MedicalRepository> logger;

    public MedicalRepository(GFADbContext _context, ILogger<MedicalRepository> logger) : base(_context)
    {
        this.logger = logger;
    }

    public async Task<EntityResult<ServiceMedical>> AddUpdateEntity(DataMedicalUpdate entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(DataMedicalUpdate));

        try
        {
            Medical? updated = null;

            if (entity.EntityId > 0 && await Entity.Include(q => q.CreatedByUser).SingleOrDefaultAsync(q => q.Id == entity.EntityId) is Medical value)
                updated = value;

            updated = entity.ToMedical(entity.GFAUser?.ToGFAUser(), updated);

            if (updated.Id == 0) Create(updated);

            await SaveChangesAsync();

            return EntityResult<ServiceMedical>.Success(updated.ToServiceMedical());
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, nameof(AddUpdateEntity), nameof(MedicalRepository));

            return EntityResult<ServiceMedical>.Failure(new[] { ex.Message });
        }
    }

    public async Task<IEnumerable<ServiceMedical>> BrowseAsync(MedicalSearchParameters parameters)
    {
        IQueryable<Medical>? query = null;

        if (parameters.Ids?.Any() ?? false)
        {
            query = FindByCondition(q => parameters.Ids.Contains(q.Id)).Include(q => q.CreatedByUser);
        }
        else
        {
            query = FindByCondition(q =>
                (string.IsNullOrWhiteSpace(parameters.SearchText) || string.IsNullOrWhiteSpace(q.Keywords) || EF.Functions.FreeText(q.Keywords, parameters.ToFreeText()!)) &&
                
                (string.IsNullOrWhiteSpace(parameters.InjuryPeriod) || parameters.InjuryYear == null || (
                    (parameters.InjuryYear == null && q.PeriodType == parameters.InjuryPeriod) ||
                    (string.IsNullOrWhiteSpace(parameters.InjuryPeriod) && q.PeriodYear == parameters.InjuryYear) ||
                    (q.PeriodType == parameters.InjuryPeriod && q.PeriodYear == parameters.InjuryYear)
                )) &&
                (parameters.ClubId == null || q.PlayerClub.ClubId == parameters.ClubId) &&
                (parameters.PlayerId == null || q.PlayerClub.PlayerId == parameters.PlayerId) &&
                (parameters.IsRecovered == null || parameters.IsRecovered == q.DateRecovered.HasValue) &&
                (parameters.InjuryStatus == null || q.InjuryStatus == parameters.InjuryStatus) &&
                (parameters.InjuryType == null || q.InjuryType == parameters.InjuryType) &&
                q.IsDeleted == parameters.IsDeleted, Top: parameters.Top
            ).Include(q => q.CreatedByUser);
        }

        query = query.Include(q => q.PlayerClub).ThenInclude(q => q.Club);

        var inc = query.Include(q => q.PlayerClub);
        if (parameters.ShowPlayer ?? false) query = inc.ThenInclude(q => q.Player);

        if (query == null) return Enumerable.Empty<ServiceMedical>();

        return (await query.ToArrayAsync()).ToServiceMedicals();
    }

    public async Task<EntityResult<ServiceMedical>> CloneAsync(DataMedicalUpdate updated)
    {
        try
        {
            if (await Entity.Where(q => q.Id == updated.EntityId).Include(q => q.CreatedByUser).SingleOrDefaultAsync() is Medical medical)
            {
                if (updated == null)
                    throw new ArgumentNullException("Update Model cannot be null", nameof(DataValueCodeTypeUpdate));

                var nw = medical.CloneModelInt(updated.GFAUser.Id, cloned => updated.ToMedical(updated.GFAUser?.ToGFAUser(), cloned));

                if (nw != null)
                {                    
                    Create(nw);

                    await SaveChangesAsync();

                    return EntityResult<ServiceMedical>.Success(medical.ToServiceMedical());
                }

                logger.LogError($"Cloning model {updated} Failed", nameof(MedicalRepository), nameof(CloneAsync));

                return EntityResult<ServiceMedical>.Failure(new[] { "Cloning model failed" });
            }

            throw new ArgumentNullException("Medical History does not exist");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, nameof(MedicalRepository), nameof(CloneAsync));

            return EntityResult<ServiceMedical>.Failure(new[] { ex.Message });
        }
    }

    public async Task<ServiceMedical?> GetAsync(int id)
    {
        return await FindByCondition(q => q.Id == id).Include(q => q.CreatedByUser).SingleOrDefaultAsync() is
            Medical medical ? medical.ToServiceMedical() : default;
    }

    public async Task<EnumerableEntityResult<ServiceMedical>> SetStatus(int[] ids, BlockStatus status, ServiceGFAUser user)
    {
        try
        {
            if (ids.Any() && await FindByCondition(q => ids.Contains(q.Id), trackChanges: true).ToArrayAsync() is Medical[] medicals && medicals.Any())
            {
                if (status == BlockStatus.Deleted)
                {
                    dbcontext.RemoveRange(medicals);
                }
                else
                {
                    foreach (var item in medicals)
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

                return EnumerableEntityResult<ServiceMedical>.Success(medicals.ToServiceMedicals());
            }

            throw new ArgumentNullException("Ids Provided cannot be null");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, nameof(ClubRepository), nameof(SetStatus));

            return EnumerableEntityResult<ServiceMedical>.Failure(new[] { ex.Message });
        }
    }
}
