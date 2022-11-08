namespace Server.Shared.Models;

public class EnumerableEntityResult<T> : EntityResult
{
    public EnumerableEntityResult(IEnumerable<T>? results, bool isPassed, IEnumerable<string>? errors = null) : base(isPassed, errors)
    {
        Results = results;
    }

    public IEnumerable<T>? Results { get; }

    public static EnumerableEntityResult<T> Failure(IEnumerable<string> errors) => new(default, false, errors);
}
