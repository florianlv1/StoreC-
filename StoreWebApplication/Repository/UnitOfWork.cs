using StoreWebApplication.DataAccess.Data;
using StoreWebApplication.Repository.IRepository;

namespace StoreWebApplication.Repository;

public class UnitOfWork : IUnitOfWork
{
    
    private ApplicationDbContext _dbContext;
    public ICategoryRepository CategoryRepository { get; private set; }
    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        CategoryRepository = new CategoryRepository(_dbContext);
    }
    

    public void Save()
    {
        _dbContext.SaveChanges();
    }
}