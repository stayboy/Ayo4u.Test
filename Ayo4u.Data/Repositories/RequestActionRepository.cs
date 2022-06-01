using Ayo4u.Data.Extensions;
using Ayo4u.Data.Models;
using Ayo4u.Infrastructure.Models;
using Ayo4u.Infrastructure.Queries;
using Ayo4u.Infrastructure.Repositories;
using Ayo4u.Server.Shared.Constants;
using Ayo4u.Server.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Ayo4u.Data.Repositories;

internal class RequestActionRepository : DBRepositoryBase<RequestAction, AyoDbContext>, IRequestActionRepository
{
    public RequestActionRepository(AyoDbContext _context) : base(_context)
    {
    }

    public async Task<EntityResult<ServiceRequestAction>> AddUpdateRequestAction(DataRequestActionUpdate request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(DataRequestActionUpdate));

        RequestAction? updated = null;
        if (request.Id > 0 && await Entity.Include(q => q.RequestAyoUser).Include(q => q.CreatedByUser)
            .Include(q => q.Conversion).SingleOrDefaultAsync(q => q.Id == request.Id) is RequestAction value)
            updated = value;

        updated = request.ToRequestAction(request.AyoUser?.ToAyoUser(), updated);

        if (updated.Id == 0) Create(updated);

        await SaveChangesAsync();

        return EntityResult<ServiceRequestAction>.Success(updated.ToServiceRequestAction());
    }

    public async Task<IEnumerable<ServiceRequestAction>> BrowseAsync(RequestActionSearchParameters parameters)
    {
        IEnumerable<RequestAction>? results = null;

        if (parameters.Ids?.Any() ?? false)
        {
            results = await FindByCondition(q => parameters.Ids.Contains(q.Id)).Include(q => q.Conversion).Include(q => q.CreatedByUser).ToArrayAsync();
        }
        else
        {
            results = await FindByCondition(q =>
                (string.IsNullOrWhiteSpace(parameters.InUnitType) || string.Equals(q.Conversion!.InUnitType, parameters.InUnitType)) &&
                (string.IsNullOrWhiteSpace(parameters.OutUnitType) || string.Equals(q.Conversion!.OutUnitType, parameters.OutUnitType)) &&

                (parameters.RequestAyoUserId == null || q.RequestAyoUserId == parameters.RequestAyoUserId) &&

                (parameters.StartCreatedAt == null || (
                    (parameters.EndCreatedAt == null && q.CreatedAt.Date == parameters.EndCreatedAt) || 
                    (parameters.StartCreatedAt >= q.CreatedAt.Date && parameters.EndCreatedAt <= q.CreatedAt.Date)
                )) &&
               
                (string.IsNullOrWhiteSpace(parameters.RequesterName) || (
                    (q.RequestAyoUser.FirstName + " " + q.RequestAyoUser.LastName).Contains(parameters.RequesterName)
                )) && q.IsDeleted == parameters.IsDeleted
            ).Include(q => q.Conversion).Include(q => q.CreatedByUser).ToArrayAsync();
        }

        if (results == null) return Enumerable.Empty<ServiceRequestAction>();

        return results.ToServiceRequestActions();
    }

    public async Task<EnumerableEntityResult<ServiceRequestAction>> DeleteLogs(int[] ids, BlockStatus status)
    {
        if (new[] {BlockStatus.Activate, BlockStatus.Clone}.Any(q => q == status))
        {
            throw new InvalidOperationException("Method not Allowed");
        }

        if (ids.Any() && await FindByCondition(q => ids.Contains(q.Id), trackChanges: true).ToArrayAsync() is RequestAction[] logs && logs.Any())
        {
            if (status ==  BlockStatus.Deleted)
            {
                dbcontext.RemoveRange(logs);
            } else
            {
                foreach (var item in logs) item.IsDeleted = true;
            }

            await SaveChangesAsync();

            return EnumerableEntityResult<ServiceRequestAction>.Success(logs.ToServiceRequestActions());
        }

        throw new ArgumentNullException("Ids Provided cannot be null");
    }

    public async Task<ServiceRequestAction?> GetAsync(int id)
    {
        return await FindByCondition(q => q.Id == id).Include(q => q.CreatedByUser).Include(q => q.RequestAyoUser).SingleOrDefaultAsync() is
            RequestAction log ? log.ToServiceRequestAction() : default;
    }
}
