using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayo4u.Web.Shared.Queries
{
    public class ApiRequestLogSearchParameters
    {
        public int[]? ids { get; set; }

        public Guid? userId { get; set; }

        public string? userName { get; set; }

        public string? inUnit { get; set; }

        public string? outUnit { get; set; }

        public DateTime? FromCreatedAt { get; set; }

        public DateTime? toCreatedAt { get; set; }

        public bool deleted { get; set; } = false;
    }
}
