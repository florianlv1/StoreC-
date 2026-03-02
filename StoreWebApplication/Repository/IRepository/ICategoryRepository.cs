using StoreWebApplication.Models;

namespace StoreWebApplication.Repository.IRepository;


public interface ICategoryRepository : IRepository<Category>
{
    void Update(Category category);

}