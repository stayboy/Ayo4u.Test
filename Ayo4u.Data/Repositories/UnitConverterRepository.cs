using Ayo4u.Data.Extensions;
using Ayo4u.Data.Models;
using Ayo4u.Infrastructure.Models;
using Ayo4u.Infrastructure.Queries;
using Ayo4u.Infrastructure.Repositories;
using Ayo4u.Server.Shared.Constants;
using Ayo4u.Server.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Ayo4u.Data.Repositories;

internal class UnitConverterRepository : DBRepositoryBase<UnitConverter, AyoDbContext>, IConverterRepository
{
    public UnitConverterRepository(AyoDbContext _context) : base(_context)
    {
    }

    public async Task<EntityResult<ServiceUnitConverter>> AddUpdateConverter(DataUnitConverterUpdate converter)
    {
        ArgumentNullException.ThrowIfNull(converter, nameof(DataUnitConverterUpdate));

        UnitConverter? updated = null;
        if (converter.Id > 0 && await Entity.SingleOrDefaultAsync(q => q.Id == converter.Id) is UnitConverter value)
            updated = value;

        updated = converter.ToUnitConverter(converter.AyoUser.ToAyoUser(), updated);

        if (updated.Id == 0) Create(updated);

        await SaveChangesAsync();

        return EntityResult<ServiceUnitConverter>.Success(updated.ToServiceUnitConverter());
    }

    public async Task<IEnumerable<ServiceUnitConverter>> BrowseAsync(UnitConverterSearchParameters parameters)
    {
        IQueryable<UnitConverter>? query = null;

        if (parameters.Ids?.Any() ?? false)
        {
            query = FindByCondition(q => parameters.Ids.Contains(q.Id)).Include(q => q.CreatedByUser);
        } else
        {
            query = FindByCondition(q =>
                (string.IsNullOrWhiteSpace(parameters.InUnitType) || string.Equals(q.InUnitType, parameters.InUnitType)) &&
                (string.IsNullOrWhiteSpace(parameters.OutUnitType) || string.Equals(q.OutUnitType, parameters.OutUnitType)) &&
                (string.IsNullOrWhiteSpace(parameters.SearchText) || (
                    q.OutUnitType.Contains(parameters.SearchText) || q.InUnitType.Contains(parameters.SearchText)
                )) && q.IsDeleted == parameters.IsDeleted
            ).Include(q => q.CreatedByUser);
        }

        if (parameters.IncludeTopLogs > 0)
        {
            query = query.Include(q => q.RequestLogs.OrderByDescending(q => q.CreatedAt).Take(parameters.IncludeTopLogs.Value));
        }

        var results = await query.ToArrayAsync();

        if (results == null) return Enumerable.Empty<ServiceUnitConverter>();

        return results.ToServiceUnitConverters();
    }

    public async Task<ServiceUnitConverter> GetAsync(int id)
    {
        if (await FindByCondition(q => q.Id == id).Include(q => q.CreatedByUser).SingleOrDefaultAsync() is UnitConverter result)
            return result.ToServiceUnitConverter();

        return default!;
    }

    public async Task<EntityResult<ServiceUnitConverter>> SetStatus(int id, BlockStatus status, DataUnitConverterUpdate converter)
    {
        if (await FindByCondition(q => q.Id == id).Include(q => q.CreatedByUser).SingleOrDefaultAsync() is UnitConverter result)
        {
            if (status == BlockStatus.Deleted)
            {
                Delete(result);
            } else
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

            return EntityResult<ServiceUnitConverter>.Success(result.ToServiceUnitConverter());
        }

        return EntityResult<ServiceUnitConverter>.Failure(new[] { "Operation Failed" });
    }
}
