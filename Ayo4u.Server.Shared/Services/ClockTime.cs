using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayo4u.Server.Shared.Services;

internal class ClockTime : IClock
{
    public DateTime Now() => DateTime.UtcNow;        
}
