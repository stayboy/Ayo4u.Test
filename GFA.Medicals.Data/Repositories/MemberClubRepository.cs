using GFA.Medicals.Data.Models;

namespace GFA.Medicals.Data.Repositories
{
    internal class MemberClubRepository : DBRepositoryBase<MemberClub, GFADbContext>, IMemberClubRepository
    {
        private readonly ILogger<MemberClubRepository> logger;

        public MemberClubRepository(GFADbContext _context, ILogger<MemberClubRepository> logger) : base(_context)
        {
            this.logger = logger;
        }

        public async Task<EntityResult<ServiceMemberClub>> AddUpdateEntity(DataMemberClubUpdate entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(DataMemberClubUpdate));

            try
            {
                MemberClub? updated = null;

                if (entity.EntityId > 0 && await Entity.Include(q => q.CreatedByUser).SingleOrDefaultAsync(q => q.Id == entity.EntityId) is MemberClub value)
                    updated = value;

                updated = entity.ToMemberClub(entity.GFAUser?.ToGFAUser(), updated);

                if (updated.Id == 0) Create(updated);

                await SaveChangesAsync();

                return EntityResult<ServiceMemberClub>.Success(updated.ToServiceMemberClub());
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, nameof(AddUpdateEntity), nameof(MemberClubRepository));

                return EntityResult<ServiceMemberClub>.Failure(new[] { ex.Message });
            }

        }

        public async Task<IEnumerable<ServiceMemberClub>> BrowseAsync(MemberClubSearchParameters parameters)
        {
            IQueryable<MemberClub>? query = null;

            if (parameters.Ids?.Any() ?? false)
            {
                query = FindByCondition(q => parameters.Ids.Contains(q.Id)).Include(q => q.CreatedByUser);
            }
            else
            {
                query = FindByCondition(q =>
                    (string.IsNullOrWhiteSpace(parameters.SearchText) || string.IsNullOrWhiteSpace(q.Keywords) || EF.Functions.FreeText(q.Keywords, parameters.ToFreeText()!)) &&
                    (parameters.CoachId == null || (q.SigningCoachId == parameters.CoachId || q.SellingCoachId == parameters.CoachId)) &&
                    (parameters.PlayerId == null || q.PlayerId == parameters.PlayerId) &&
                    (parameters.Exited == null || parameters.Exited == q.DateExited.HasValue) &&
                    (parameters.StartJoinDate == null || parameters.EndJoinDate == null || (
                        (parameters.EndJoinDate == null && q.DateJoined.Date >= parameters.StartJoinDate) || 
                        (q.DateJoined.Date >= parameters.StartJoinDate && q.DateJoined.Date <= parameters.EndJoinDate)
                    )) &&
                    (parameters.StartExitDate == null || parameters.EndExitDate == null || q.DateExited == null || (
                        (parameters.EndExitDate == null && q.DateExited.Value.Date >= parameters.StartExitDate) ||
                        (q.DateExited.Value.Date >= parameters.StartExitDate && q.DateExited.Value.Date <= parameters.EndExitDate)
                    )) &&
                    q.IsDeleted == parameters.IsDeleted, Top: parameters.Top
                ).Include(q => q.CreatedByUser);
            }

            if (query == null) return Enumerable.Empty<ServiceMemberClub>();

            return (await query.ToArrayAsync()).ToServiceMemberClubs();
        }

        public async Task<EntityResult<ServiceMemberClub>> CloneAsync(DataMemberClubUpdate updated)
        {
            try
            {
                if (await Entity.Where(q => q.Id == updated.EntityId).Include(q => q.CreatedByUser).SingleOrDefaultAsync() is MemberClub club)
                {
                    if (updated == null)
                        throw new ArgumentNullException("Update Model cannot be null", nameof(DataMemberClubUpdate));

                    var nw = club.CloneModelInt(updated.GFAUser.Id, cloned => updated.ToMemberClub(updated.GFAUser?.ToGFAUser(), cloned));

                    if (nw != null)
                    {
                        Create(nw);

                        await SaveChangesAsync();

                        return EntityResult<ServiceMemberClub>.Success(club.ToServiceMemberClub());
                    }

                    logger.LogError($"Cloning model {updated} Failed", nameof(MemberClubRepository), nameof(CloneAsync));

                    return EntityResult<ServiceMemberClub>.Failure(new[] { "Cloning model failed" });
                }

                throw new ArgumentNullException("Club Registration does not exist");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, nameof(MemberClubRepository), nameof(AddUpdateEntity));

                return EntityResult<ServiceMemberClub>.Failure(new[] { ex.Message });
            }
        }

        public async Task<ServiceMemberClub?> GetAsync(int id)
        {
            return await FindByCondition(q => q.Id == id).Include(q => q.CreatedByUser).SingleOrDefaultAsync() is
                        MemberClub club ? club.ToServiceMemberClub() : default;
        }

        public async Task<EnumerableEntityResult<ServiceMemberClub>> SetStatus(int[] ids, BlockStatus status, ServiceGFAUser user)
        {
            try
            {
                if (ids.Any() && await FindByCondition(q => ids.Contains(q.Id), trackChanges: true).ToArrayAsync() is MemberClub[] clubs && clubs.Any())
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

                    return EnumerableEntityResult<ServiceMemberClub>.Success(clubs.ToServiceMemberClubs());
                }

                throw new ArgumentNullException("Ids Provided cannot be null");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, nameof(MemberClubRepository), nameof(SetStatus));

                return EnumerableEntityResult<ServiceMemberClub>.Failure(new[] { ex.Message });
            }
        }
    }
}
