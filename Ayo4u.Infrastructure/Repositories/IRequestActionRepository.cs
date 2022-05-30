using Ayo4u.Infrastructure.Models;
using Ayo4u.Infrastructure.Queries;
using Ayo4u.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayo4u.Infrastructure.Repositories;

public interface IRequestActionRepository
{
    Task<EntityResult<ServiceRequestAction>> AddUpdateRequestAction(DataRequestActionUpdate request);

    Task<IEnumerable<ServiceRequestAction>> BrowseAsync(RequestActionSearchParameters parameters);
}
