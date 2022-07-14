using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Shared.Models
{
    [NotMapped]
    public abstract class IdModel<T, U, UserModel> where U : class where UserModel : IUserProfile<Guid>
    {
        public T Id { get; set; } = default!;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Guid? CreatedByUserId { get; set; }

        public Guid? ModifiedByUserId { get; set; }

        public UserModel? CreatedByUser { get; set; }

        public UserModel? ModifiedByUser { get; set; }

        public bool IsDeleted { get; set; } = true;

        public T? PreviousId { get; set; }

        public U? Previous { get; set; }

        public string? Keywords { get; set; }
    }
}
