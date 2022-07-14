namespace GFA.Medicals.Web.Shared.Models;

public abstract record ApiChange<T>(T Id, DateTime? Created = null);
