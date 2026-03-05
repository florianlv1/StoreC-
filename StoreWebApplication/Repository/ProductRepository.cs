using System.Linq.Expressions;
using StoreWebApplication.DataAccess.Data;
using StoreWebApplication.Models;
using StoreWebApplication.Repository.IRepository;

namespace StoreWebApplication.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private ApplicationDbContext _dbContext;

    public ProductRepository(ApplicationDbContext context) : base(context)
    {
        _dbContext = context;
    }
    
    
    public void Update(Product Product)
    {
        _dbContext.Update(Product);
    }
    
}