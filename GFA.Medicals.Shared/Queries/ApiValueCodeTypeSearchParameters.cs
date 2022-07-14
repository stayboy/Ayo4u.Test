using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFA.Medicals.Web.Shared.Queries
{
    public class ApiValueCodeTypeSearchParameters : ApiSearchParameters<int>
    {
        public string[]? parentcodes { get; set; }

        public string[]? childcodes { get; set; }
    }
}
