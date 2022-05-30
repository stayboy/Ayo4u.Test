namespace Ayo4u.Server.Shared.Models;

public abstract class EntityResult
{
    public EntityResult(bool isPassed, IEnumerable<string>? errors = null)
    {
        IsSuccessful = isPassed;

        Errors = errors;
    }

    public bool IsSuccessful { get; }

    public IEnumerable<string>? Errors { get; }
}

public class EntityResult<T> : EntityResult where T : class
{
    public EntityResult(T? result, bool isPassed, IEnumerable<string>? errors = null) : base(isPassed, errors)
    {
        Result = result;
    }

    public T? Result { get; }

    public static EntityResult<T> Success(T dto) => new(dto, true);

    public static EntityResult<T> Failure(IEnumerable<string> errors) => new(null, false, errors);
}
