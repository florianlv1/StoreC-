using System.Linq.Expressions;
using StoreWebApplication.DataAccess.Data;
using StoreWebApplication.Models;
using StoreWebApplication.Repository.IRepository;

namespace StoreWebApplication.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private ApplicationDbContext _dbContext;

    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
        _dbContext = context;
    }
    
    
    public void Update(Category category)
    {
        _dbContext.Update(category);
    }
    
}