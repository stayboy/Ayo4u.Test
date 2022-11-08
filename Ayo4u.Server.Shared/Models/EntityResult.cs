namespace Server.Shared.Models;

public abstract class EntityResult
{
    public EntityResult(bool isPassed, IEnumerable<string>? errors = null)
    {
        IsSuccessful = isPassed;

        Errors = errors;
    }

    public bool IsSuccessful { get; }

    public IEnumerable<string>? Errors { get; }

    public static EntityResult<T> Default<T>(T dto) => (dto == null || default(T)!.Equals(dto)) ? Failure<T>(new[] { "No Result Found" }) : Success(dto);

    public static EnumerableEntityResult<T> Default<T>(IEnumerable<T> dto) => (dto == null || default(T)!.Equals(dto)) ? new(Enumerable.Empty<T>(), false, new[] { "No Result Found" }) : Success(dto);

    public static EntityResult<T> Success<T>(T dto) => new(dto, true);

    public static EntityResult<T> Failure<T>(IEnumerable<string> errors) => new(default, false, errors);

    public static EntityResult<T> Failure<T>(Exception ex) => new(default, false, new[] { ex.Message });

    public static EnumerableEntityResult<T> Success<T>(IEnumerable<T> dto) => new(dto, true);
}

public class EntityResult<T> : EntityResult
{
    public EntityResult(T? result, bool isPassed, IEnumerable<string>? errors = null) : base(isPassed, errors)
    {
        Result = result;
    }

    public T? Result { get; }
}
