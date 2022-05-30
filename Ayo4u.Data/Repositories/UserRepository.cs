using Ayo4u.Data.Extensions;
using Ayo4u.Data.Models;
using Ayo4u.Infrastructure.Models;
using Ayo4u.Infrastructure.Queries;
using Ayo4u.Infrastructure.Repositories;
using Ayo4u.Server.Shared.Constants;
using Ayo4u.Server.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayo4u.Data.Repositories
{
    internal class UserRepository : DBRepositoryBase<AyoUser, AyoDbContext>, IUserRepository
    {
        public UserRepository(AyoDbContext _context) : base(_context)
        {
        }

        public async Task<EntityResult<ServiceAyoUser>> AddUpdateUser(DataAyoUserUpdate user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(DataAyoUserUpdate));

            AyoUser? updated = null;
            if (user.Id != Guid.Empty && await Entity.SingleOrDefaultAsync(q => q.Id == user.Id) is AyoUser value)
                updated = value;

            updated = user.ToAyoUserUpdate(user.AyoUser.ToAyoUser(), updated);

            if (user.Id == Guid.Empty) Create(updated);

            await SaveChangesAsync();

            return EntityResult<ServiceAyoUser>.Success(updated.ToServiceAyoUser());
        }

        public Task<IEnumerable<ServiceAyoUser>> BrowseAsync(UserSearchParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceAyoUser> Get(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceAyoUser> Get(string email)
        {
            throw new NotImplementedException();
        }

        public Task<EntityResult<ServiceAyoUser>> SetStatus(Guid userId, BlockStatus status, DataAyoUserUpdate user)
        {
            throw new NotImplementedException();
        }
    }
}
