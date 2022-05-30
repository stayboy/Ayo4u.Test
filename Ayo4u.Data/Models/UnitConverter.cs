using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayo4u.Data.Models;

internal class UnitConverter : IdModel<int, UnitConverter>
{
    public string InUnitType { get; set; } = default!;

    public string OutUnitType { get; set; } = default!;

    public float Multiplier { get; set; }

    public virtual IReadOnlyCollection<RequestAction> RequestLogs { get; set; } = new HashSet<RequestAction>();
}
