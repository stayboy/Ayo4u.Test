using System.ComponentModel.DataAnnotations.Schema;

namespace GFA.Medicals.Data.Models;

[NotMapped]
internal abstract class GFAIdModel<T, U> : IdModel<T, U, GFAUser> where U : class
{
}
