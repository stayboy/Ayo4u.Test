namespace GFA.Medicals.Data.Repositories;

internal class CodeTypeRepository : DBRepositoryBase<ValueCodeType, GFADbContext>, ICodeTypeRepository
{
    private readonly ILogger<CodeTypeRepository> logger;

    public CodeTypeRepository(GFADbContext _context, ILogger<CodeTypeRepository> logger) : base(_context)
    {
        this.logger = logger;
    }

    public async Task<EntityResult<ServiceValueCodeType>> AddUpdateEntity(DataValueCodeTypeUpdate entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(DataValueCodeTypeUpdate));

        try
        {
            ValueCodeType? updated = null;

            if (entity.EntityId > 0 && await Entity.Include(q => q.CreatedByUser).SingleOrDefaultAsync(q => q.Id == entity.EntityId) is ValueCodeType value)
                updated = value;

            updated = entity.ToValueCodeType(entity.GFAUser?.ToGFAUser(), updated);

            if (updated.Id == 0) Create(updated);

            await SaveChangesAsync();

            return EntityResult<ServiceValueCodeType>.Success(updated.ToServiceValueCodeType());
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, nameof(AddUpdateEntity), nameof(CodeTypeRepository));

            return EntityResult<ServiceValueCodeType>.Failure(new[] { ex.Message });
        }
    }

    public async Task<IEnumerable<ServiceValueCodeType>> BrowseAsync(ValueCodeTypeSearchParameters parameters)
    {
        IQueryable<ValueCodeType>? query = null;

        if (parameters.Ids?.Any() ?? false)
        {
            query = FindByCondition(q => parameters.Ids.Contains(q.Id)).Include(q => q.CreatedByUser);
        }
        else
        {
            query = FindByCondition(q =>
                (string.IsNullOrWhiteSpace(parameters.SearchText) || string.IsNullOrWhiteSpace(q.Keywords) || EF.Functions.FreeText(q.Keywords, parameters.ToFreeText()!)) &&
                (parameters.CodeTypes == null || parameters.CodeTypes.Contains(q.CodeType)) &&
                (parameters.Codes == null || parameters.Codes.Contains(q.Code)) &&
                q.IsDeleted == parameters.IsDeleted, Top: parameters.Top
            ).Include(q => q.CreatedByUser);
        }

        if (query == null) return Enumerable.Empty<ServiceValueCodeType>();

        return (await query.ToArrayAsync()).ToServiceValueTypeCodes();
    }

    public async Task<EntityResult<ServiceValueCodeType>> CloneAsync(DataValueCodeTypeUpdate updated)
    {
        try
        {
            if (await Entity.Where(q => q.Id == updated.EntityId).Include(q => q.CreatedByUser).SingleOrDefaultAsync() is ValueCodeType code)
            {
                if (updated == null)
                    throw new ArgumentNullException("Update Model cannot be null", nameof(DataValueCodeTypeUpdate));

                var nw = code.CloneModelInt(updated.GFAUser.Id, cloned => updated.ToValueCodeType(updated.GFAUser.ToGFAUser(), cloned));

                if (nw != null)
                {                   
                    Create(nw);

                    await SaveChangesAsync();

                    return EntityResult<ServiceValueCodeType>.Success(code.ToServiceValueCodeType());
                }

                logger.LogError($"Cloning model {updated} Failed", nameof(CodeTypeRepository), nameof(CloneAsync));

                return EntityResult<ServiceValueCodeType>.Failure(new[] { "Cloning model failed" });
            }

            throw new ArgumentNullException("Code does not exist");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, nameof(CodeTypeRepository), nameof(CloneAsync));

            return EntityResult<ServiceValueCodeType>.Failure(new[] { ex.Message });
        }
    }

    public async Task<ServiceValueCodeType?> GetAsync(int id)
    {
        return await FindByCondition(q => q.Id == id).Include(q => q.CreatedByUser).SingleOrDefaultAsync() is
            ValueCodeType code ? code.ToServiceValueCodeType() : default;
    }

    public async Task<EnumerableEntityResult<ServiceValueCodeType>> SetStatus(int[] ids, BlockStatus status, ServiceGFAUser user)
    {
        try
        {
            if (ids.Any() && await FindByCondition(q => ids.Contains(q.Id), trackChanges: true).ToArrayAsync() is ValueCodeType[] codes && codes.Any())
            {
                if (status == BlockStatus.Deleted)
                {
                    dbcontext.RemoveRange(codes);
                }
                else
                {
                    foreach (var item in codes)
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

                return EnumerableEntityResult<ServiceValueCodeType>.Success(codes.ToServiceValueTypeCodes());
            }

            throw new ArgumentNullException("Ids Provided cannot be null");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, nameof(ClubRepository), nameof(SetStatus));

            return EnumerableEntityResult<ServiceValueCodeType>.Failure(new[] { ex.Message });
        }
    }
}
