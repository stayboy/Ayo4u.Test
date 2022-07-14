namespace GFA.Medicals.Infrastructure.Repositories;

public interface IRepository<B, T, S, U> where T : BaseEntity<B> where S : SearchParameters<B> where U : DataUpdate<B> where B : struct
{
    Task<EntityResult<T>> AddUpdateEntity(U entity);

    virtual Task<T?> GetAsync(B id) => throw new NotImplementedException();

    Task<IEnumerable<T>> BrowseAsync(S parameters);

    Task<EnumerableEntityResult<T>> SetStatus(B[] ids, BlockStatus status, ServiceGFAUser user);

    Task<EntityResult<T>> CloneAsync(U updated);
}


public interface IIntRepository<T, S, U> : IRepository<int, T, S, U> where T : BaseEntity<int> where S : SearchParameters<int> where U : DataUpdate<int>
{
}

public interface IGUIDRepository<T, S, U> : IRepository<Guid, T, S, U> where T : BaseEntity<Guid> where S : SearchParameters<Guid> where U : DataUpdate<Guid>
{
}
