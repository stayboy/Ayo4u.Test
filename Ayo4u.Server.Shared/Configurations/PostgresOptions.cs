using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayo4u.Server.Shared.Configurations
{
    public class PostgresOptions
    {
        public const string Postgres = "postgres";

        public string ConnectionString { get; set; } = default!;
    }
}
