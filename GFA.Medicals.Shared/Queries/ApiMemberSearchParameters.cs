using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFA.Medicals.Web.Shared.Queries
{
    public class ApiMemberSearchParameters : ApiSearchParameters<int>
    {
        public string? person { get; set; }

        public string? sex { get; set; }

        public int? club { get; set; }

        public int? country { get; set; }

        public bool? latestclub { get; set; }

        public int? injury { get; set; }

        public bool? fit { get; set; }

        public int? minage { get; set; }

        public int? maxage { get; set; }

        public string? injuryperiod { get; set; }

        public int? injuryyear { get; set; }

        public bool? showmedicals { get; set; }

        public bool? showclubs { get; set; }
    }
}
