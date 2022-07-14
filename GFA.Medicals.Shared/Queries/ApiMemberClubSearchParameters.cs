using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFA.Medicals.Web.Shared.Queries
{
    public class ApiMemberClubSearchParameters : ApiSearchParameters<int>
    {
        public int? coach { get; set; }

        public int? player { get; set; }

        public bool? exited { get; set; }

        public DateTime? joinstartdate { get; set; }

        public DateTime? joinenddate { get; set; }

        public DateTime? exitstartdate { get; set; }

        public DateTime? exitenddate { get; set; }
    }
}
