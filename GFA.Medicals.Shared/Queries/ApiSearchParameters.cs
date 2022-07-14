using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFA.Medicals.Web.Shared.Queries
{
    public abstract class ApiSearchParameters<T> where T: struct
    {
        public T[]? ids { get; set; }

        public string? search { get; set; }

        public bool deleted { get; set; }

        public int top { get; set; } = 20;
    }
}
