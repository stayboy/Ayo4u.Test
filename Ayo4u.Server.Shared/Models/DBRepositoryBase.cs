using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ayo4u.Server.Shared.Models;

public abstract class DBRepositoryBase<T, TContext> where T : class where TContext : DbContext
{
    protected readonly TContext dbcontext;
    private readonly DbSet<T> _entity;

    public DBRepositoryBase(TContext _context)
    {
        dbcontext = _context;

        _entity = dbcontext.Set<T>();
    }

    public DbSet<T> Entity => dbcontext.Set<T>();

    public void Create(T entity) => dbcontext.Set<T>().Add(entity);

    public void Delete(T entity) => dbcontext.Set<T>().Remove(entity);

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, int? Top = 100, bool trackChanges = false) =>
        !trackChanges ? _entity.Where(expression).AsNoTracking().Take(Top ?? 100) : _entity.Where(expression).Take((Top ?? 100));

    public void Update(T entity) => dbcontext.Set<T>().Update(entity);

    public void UpdateValues<U>(T entity, U Values) => dbcontext.Entry(entity).CurrentValues.SetValues(Values!);

    public Task<int> SaveChangesAsync() => dbcontext.SaveChangesAsync();
}
