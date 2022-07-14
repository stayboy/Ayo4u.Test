using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared.Services
{
    public interface IClock
    {
        DateTime Now();
    }
}
