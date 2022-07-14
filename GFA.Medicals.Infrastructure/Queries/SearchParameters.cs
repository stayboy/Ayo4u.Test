using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFA.Medicals.Infrastructure.Queries
{
    public abstract class SearchParameters<T>
    {
        public T[]? Ids { get; set; }

        public string? SearchText { get; set; }

        public bool IsDeleted { get; set; }

        public int Top { get; set; } = 20;

        public string? SortField { get; set; }

        public int? PageIndex { get; set; }

        public string ToFreeText()
        {
            if (string.IsNullOrWhiteSpace(SearchText)) return "*";
            
            var arr = SearchText.Split(' ').Select(x => x.TrimEnd() + '*');
            var str = (SearchText ?? "*");

            if (arr.Count() > 1) str = $"(\"{string.Join("\" AND \"", arr)})";
            else str = "\"" + SearchText + "*\"";

            return str;
        }
    }
}
