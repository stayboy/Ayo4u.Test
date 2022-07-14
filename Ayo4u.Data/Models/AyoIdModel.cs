using System.ComponentModel.DataAnnotations.Schema;

namespace Ayo4u.Data.Models;

[NotMapped]
internal abstract class AyoIdModel<T, U> : IdModel<T, U, AyoUser> where U : class
{
}
