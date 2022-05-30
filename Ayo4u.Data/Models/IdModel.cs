using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayo4u.Data.Models
{
    [NotMapped]
    internal abstract class IdModel<T, U> where U : class
    {
        public T Id { get; set; } = default!;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Guid? CreatedByUserId { get; set; }

        public Guid? ModifiedByUserId { get; set; }

        public AyoUser? CreatedByUser { get; set; }

        public AyoUser? ModifiedByUser { get; set; }

        public bool IsDeleted { get; set; } = true;

        public T? PreviousId { get; set; }

        public U? Previous { get; set; }
    }
}
