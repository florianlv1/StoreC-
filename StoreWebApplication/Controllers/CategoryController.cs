using Microsoft.AspNetCore.Mvc;
using StoreWebApplication.Data;
using StoreWebApplication.Models;

namespace StoreWebApplication.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    public CategoryController(ApplicationDbContext dbDbContext)
    {
        _dbContext = dbDbContext;
    }
    public IActionResult Index()
    {
        List<Category> objCategoryList = _dbContext.Categories.ToList();
        
        return View(objCategoryList);
    }
}