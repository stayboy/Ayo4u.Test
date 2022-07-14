namespace Server.Shared.Models;

public interface IUserProfile<T> where T : struct
{
    public T Id { get; set; }

    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}
