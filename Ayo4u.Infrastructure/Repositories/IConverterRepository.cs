using Ayo4u.Infrastructure.Models;
using Ayo4u.Infrastructure.Queries;
using Ayo4u.Server.Shared.Constants;
using Ayo4u.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayo4u.Infrastructure.Repositories;

public interface IConverterRepository
{
    Task<ServiceUnitConverter> GetAsync(int id);

    Task<EntityResult<ServiceUnitConverter>> AddUpdateConverter(DataUnitConverterUpdate converter);

    Task<IEnumerable<ServiceUnitConverter>> BrowseAsync(UnitConverterSearchParameters parameters);

    Task<EntityResult<ServiceUnitConverter>> SetStatus(int id, BlockStatus status, DataUnitConverterUpdate converter);
}
