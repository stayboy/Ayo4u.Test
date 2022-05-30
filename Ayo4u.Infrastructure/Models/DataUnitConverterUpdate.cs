using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayo4u.Infrastructure.Models
{
    public record DataUnitConverterUpdate (int Id, DateTime CreatedAt)
    {
        public string InUnitType { get; set; } = default!;

        public string OutUnitType { get; set; } = default!;

        public float Multiplier { get; set; }

        public ServiceAyoUser AyoUser { get; set; } = default!;
    }
}
