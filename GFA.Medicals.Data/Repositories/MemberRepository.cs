namespace GFA.Medicals.Data.Repositories;

internal class MemberRepository : DBRepositoryBase<Member, GFADbContext>, IMemberRepository
{
    private readonly ILogger<MemberRepository> logger;

    public MemberRepository(GFADbContext _context, ILogger<MemberRepository> logger) : base(_context)
    {
        this.logger = logger;
    }

    public async Task<EntityResult<ServiceMember>> AddUpdateEntity(DataMemberUpdate entity)
    {
        try
        {
            Member? updated = null;

            if (entity.EntityId > 0 && await Entity.Include(q => q.CreatedByUser).SingleOrDefaultAsync(q => q.Id == entity.EntityId) is Member value)
                updated = value;

            updated = entity.ToMember(entity.GFAUser?.ToGFAUser(), updated);

            if (updated.Id == 0) Create(updated);

            await SaveChangesAsync();

            return EntityResult.Success(updated.ToServiceMember());
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, nameof(AddUpdateEntity), nameof(MemberRepository));

            return EntityResult.Failure<ServiceMember>(new[] { ex.Message });
        }
        
    }

    public async Task<IEnumerable<ServiceMember>> BrowseAsync(MemberSearchParameters parameters)
    {
        IQueryable<Member>? query = null;

        if (parameters.Ids?.Any() ?? false)
        {
            query = FindByCondition(q => parameters.Ids.Contains(q.Id)).Include(q => q.CreatedByUser);
        }

        query ??= FindByCondition(q =>
            (string.IsNullOrWhiteSpace(parameters.SearchText) || string.IsNullOrWhiteSpace(q.Keywords) || EF.Functions.FreeText(q.Keywords, parameters.ToFreeText()!)) &&
            (string.IsNullOrWhiteSpace(parameters.MemberType) || q.MemberType == parameters.MemberType) &&
            (string.IsNullOrWhiteSpace(parameters.Gender) || q.Gender == parameters.Gender) &&
            (string.IsNullOrWhiteSpace(parameters.InjuryPeriod) || parameters.InjuryYear == null ||
                (parameters.InjuryYear == null && q.ClubRegisterations.Any(o => o.Medicals.Any(m => m.PeriodType == parameters.InjuryPeriod))) ||
                (string.IsNullOrWhiteSpace(parameters.InjuryPeriod) && q.ClubRegisterations.Any(o => o.Medicals.Any(m => m.PeriodYear == parameters.InjuryYear))) ||
                q.ClubRegisterations.Any(o => o.Medicals.Any(m => m.PeriodType == parameters.InjuryPeriod && m.PeriodYear == parameters.InjuryYear))
            ) &&
            (parameters.MinAgeYears == null || parameters.MaxAgeYears == null || (
                (parameters.MaxAgeYears == null && EF.Functions.DateDiffYear(q.DateOfBirth, DateTime.Today) >= parameters.MinAgeYears) ||
                (parameters.MinAgeYears >= EF.Functions.DateDiffYear(q.DateOfBirth, DateTime.Today) &&
                        EF.Functions.DateDiffYear(q.DateOfBirth, DateTime.Today) < parameters.MaxAgeYears)
            )) &&
            (parameters.ClubId == null ||
                (parameters.IsLatestClub == true && q.ClubRegisterations.Any() &&
                (q.ClubRegisterations.OrderByDescending(q => q.DateJoined).Select(x => new { InClub = x.ClubId == parameters.ClubId }).Take(1)).Any(c => c.InClub == true))
                || q.Clubs.Any(o => o.Id == parameters.ClubId)) &&
            (parameters.ClubCountryId == null ||
                    (parameters.IsLatestClub == true && q.ClubRegisterations.Any() &&
                        (q.ClubRegisterations.OrderByDescending(q => q.DateJoined)
                        .Select(x => new { IsCountry = parameters.ClubCountryId == x.Club!.CountryId }).Take(1)).Any(c => c.IsCountry == true))
                   || q.Clubs.Any(o => o.CountryId == parameters.ClubCountryId)) &&
            (parameters.InjuryType == null || (q.ClubRegisterations.OrderByDescending(o => o.DateJoined)
                .Select(x => new { IsInjured = x.Medicals.Any(m => m.InjuryType == parameters.InjuryType && m.DateRecovered == null) }).Take(1)).Any(m => m.IsInjured == true)) &&
            q.IsDeleted == parameters.IsDeleted, Top: parameters.Top
        ).Include(q => q.CreatedByUser);
        

        if (query == null) return Enumerable.Empty<ServiceMember>();

        if (parameters.LoadClubs ?? parameters.LoadMedicals ?? false)
        {
            var includes = query.Include(q => q.ClubRegisterations.Where(c =>
                        (string.IsNullOrWhiteSpace(parameters.InjuryPeriod) || parameters.InjuryYear == null ||
                                (parameters.InjuryYear == null && c.Medicals.Any(m => m.PeriodType == parameters.InjuryPeriod)) ||
                                (string.IsNullOrWhiteSpace(parameters.InjuryPeriod) && c.Medicals.Any(m => m.PeriodYear == parameters.InjuryYear)) ||
                                c.Medicals.Any(m => m.PeriodType == parameters.InjuryPeriod && m.PeriodYear == parameters.InjuryYear)
                            ) &&
                        (parameters.ClubId == null || c.ClubId == parameters.ClubId) &&
                        (parameters.ClubCountryId == null || c.Club.CountryId == parameters.ClubCountryId) &&
                        (parameters.InjuryType == null || c.Medicals.Any(m => m.InjuryType == parameters.InjuryType && m.DateRecovered == null))
                    ));

            if (parameters.LoadClubs ?? false) query = includes.ThenInclude(q => q.Club);
            if (parameters.LoadMedicals ?? false) query = includes.ThenInclude(q => q.Medicals.Where(m =>
                (parameters.InjuryType == null || m.InjuryType == parameters.InjuryType && m.DateRecovered == null) &&
                (string.IsNullOrWhiteSpace(parameters.InjuryPeriod) || parameters.InjuryYear == null ||
                                (parameters.InjuryYear == null && m.PeriodType == parameters.InjuryPeriod) ||
                                (string.IsNullOrWhiteSpace(parameters.InjuryPeriod) && m.PeriodYear == parameters.InjuryYear) ||
                                (m.PeriodType == parameters.InjuryPeriod && m.PeriodYear == parameters.InjuryYear)
                            )
            ));
        }

        if (parameters.SortFields?.Any() == true)
        {
            query = query.ApplySortingFields(parameters.SortFields);
        }

        return (await query.ToArrayAsync()).ToServiceMembers();
    }

    public async Task<EntityResult<ServiceMember>> CloneAsync(DataMemberUpdate updated)
    {
        try
        {
            if (await Entity.Where(q => q.Id == updated.EntityId).Include(q => q.CreatedByUser).SingleOrDefaultAsync() is Member member)
            {
                if (updated == null)
                    throw new ArgumentNullException("Update Model cannot be null", nameof(DataMemberUpdate));

                var nw = member.CloneModelInt(updated.GFAUser.Id, cloned => updated.ToMember(updated.GFAUser.ToGFAUser(), cloned));

                if (nw != null)
                {
                    Create(nw);

                    await SaveChangesAsync();

                    return EntityResult.Success(member.ToServiceMember());
                }

                logger.LogError($"Cloning model {updated} Failed", nameof(MemberRepository), nameof(CloneAsync));

                return EntityResult.Failure<ServiceMember>(new[] { "Cloning model failed" });
            }

            throw new ArgumentNullException("Member does not exist");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, nameof(MemberRepository), nameof(AddUpdateEntity));

            return EntityResult.Failure<ServiceMember>(new[] { ex.Message });
        }
    }

    public async Task<ServiceMember?> GetAsync(int id)
    {
        return await FindByCondition(q => q.Id == id).Include(q => q.CreatedByUser).SingleOrDefaultAsync() is
          Member member ? member.ToServiceMember() : default;
    }

    public async Task<EnumerableEntityResult<ServiceMember>> SetStatus(int[] ids, BlockStatus status, ServiceGFAUser user)
    {
        try
        {
            if (ids.Any() && await FindByCondition(q => ids.Contains(q.Id), trackChanges: true).ToArrayAsync() is Member[] members && members.Any())
            {
                if (status == BlockStatus.Deleted)
                {
                    dbcontext.RemoveRange(members);
                }
                else
                {
                    foreach (var item in members)
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

                return EntityResult.Success(members.ToServiceMembers());
            }

            throw new ArgumentNullException("Ids Provided cannot be null");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, nameof(MemberRepository), nameof(SetStatus));

            return EnumerableEntityResult<ServiceMember>.Failure(new[] { ex.Message });
        }
    }
}
