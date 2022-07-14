using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFA.Medicals.Infrastructure.Models
{
    public record DataGFAUserUpdate : DataUpdate<Guid>
    {
        public DataGFAUserUpdate(Guid EntityId, DateTime CreatedAt) : base(EntityId, CreatedAt)
        {
        }

        public string Email { get; set; } = default!;

        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;
    }
}
