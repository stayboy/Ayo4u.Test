using Ayo4u.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayo4u.Infrastructure.Models
{
    public class ServiceUnitConverter : BaseEntity<int>
    {
        public string InUnitType { get; set; } = default!;

        public string OutUnitType { get; set; } = default!;

        public float Multiplier { get; set; }

        public IEnumerable<ServiceRequestAction>? RequestLogs { get; set; }
    }
}
