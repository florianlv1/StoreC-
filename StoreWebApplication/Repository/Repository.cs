using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using StoreWebApplication.DataAccess.Data;
using StoreWebApplication.Repository.IRepository;

namespace StoreWebApplication.Repository;


public class Repository<T> : IRepository<T> where T : class
{
    
    private readonly ApplicationDbContext _dbContext;
    internal DbSet<T> DbSet;
    public Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        this.DbSet = _dbContext.Set<T>();
    }
    
    public IEnumerable<T> GetAll()
    {
        IQueryable<T> query = DbSet;
        return query.ToList();
    }

    public T Get(Expression<Func<T, bool>> filter)
    {
        IQueryable<T> query = DbSet;
        query = query.Where(filter);
        return query.FirstOrDefault();
    }

    public void Add(T entity)
    {
        DbSet.Add(entity);
    }

    public void Remove(T entity)
    {
        DbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        DbSet.RemoveRange(entities);
    }
}