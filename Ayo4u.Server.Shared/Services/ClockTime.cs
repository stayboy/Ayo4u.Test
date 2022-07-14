using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared.Services;

internal class ClockTime : IClock
{
    public DateTime Now() => DateTime.UtcNow;        
}
