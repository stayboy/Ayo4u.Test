using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayo4u.Web.Shared.Queries
{
    public class ApiConverterSearchParameters
    {
        public int[] ids { get; set; }

        public string? search { get; set; }

        public string? inUnit { get; set; }

        public string? outUnit { get; set; }

        public int? loadTopLogs { get; set; }

        public bool deleted { get; set; } = false;
    }
}
