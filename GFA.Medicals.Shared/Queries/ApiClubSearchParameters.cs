using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFA.Medicals.Web.Shared.Queries
{
    public class ApiClubSearchParameters : ApiSearchParameters<int>
    {
        public int? country { get; set; }

        public string? division { get; set; }
    }
}
