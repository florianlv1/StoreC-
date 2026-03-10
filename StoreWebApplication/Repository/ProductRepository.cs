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
    
    
    public void Update(Product obj)
    {
        var objFromDb = _dbContext.Products.FirstOrDefault(x => x.Id == obj.Id);

        if (objFromDb != null)
        {
            objFromDb.Title = obj.Title;
            objFromDb.ISBN = obj.ISBN;
            objFromDb.Price = obj.Price;
            objFromDb.Price50 = obj.Price50;
            objFromDb.Price100 = obj.Price100;
            objFromDb.ListPrice = obj.ListPrice;
            objFromDb.Description = obj.Description;
            objFromDb.Category = obj.Category;
            objFromDb.Author = obj.Author;
            if (obj.ImageUrl != null)
            {
                objFromDb.ImageUrl = obj.ImageUrl;
            }

        }
    }
    
}