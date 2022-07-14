namespace Server.Shared.Models;

public class EnumerableEntityResult<T> : EntityResult where T : class
{
    public EnumerableEntityResult(IEnumerable<T>? results, bool isPassed, IEnumerable<string>? errors = null) : base(isPassed, errors)
    {
        Results = results;
    }

    public IEnumerable<T>? Results { get; }

    public static EnumerableEntityResult<T> Success(IEnumerable<T> dto) => new(dto, true);

    public static EnumerableEntityResult<T> Failure(IEnumerable<string> errors) => new(null, false, errors);
}
