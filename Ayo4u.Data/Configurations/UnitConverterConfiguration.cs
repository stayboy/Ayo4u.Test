using Ayo4u.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayo4u.Data.Configurations
{
    internal class UnitConverterConfiguration : IdModelConfiguration<int, UnitConverter>
    {
        public UnitConverterConfiguration() : base("Converters")
        {
        }
    }
}
