using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayo4u.Web.Shared.Models
{
    public class ApiUnitConverter : ApiBaseEntity<int>
    {
        public string InUnitType { get; set; } = default!;

        public string OutUnitType { get; set; } = default!;

        public float Multiplier { get; set; }

        public IEnumerable<ApiRequestAction>? RequestLogs { get; set; }
    }
}
