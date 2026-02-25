using Microsoft.AspNetCore.Mvc;
using StoreWebApplication.DataAccess.Data;
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

    public IActionResult Create()
    {
        return View();
    }
    

    
    [HttpPost]
    public IActionResult Create(Category category)
    {
        if (category.Name == category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name", "The Name cannot exatly match the Display Order");
        }
        if (category.Name.ToLower() == "haha")
        {
            ModelState.AddModelError("", "haha is an invalid value");
        }
        if (ModelState.IsValid)
        {
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
            TempData["Message"] = "Category created successfully";
            return RedirectToAction("Index", "Category");

        }

        return View();
    }
    
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound(); 
        }
        Category category = _dbContext.Categories.Find(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }
    
    [HttpPost]
    public IActionResult Edit(Category category)
    {
        if (ModelState.IsValid)
        {
            _dbContext.Categories.Update(category);
            _dbContext.SaveChanges();
            TempData["Message"] = "Category updated successfully";
            return RedirectToAction("Index", "Category");

        }

        return View();
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        Category category = _dbContext.Categories.Find(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? id)
    {
        Category category = _dbContext.Categories.Find(id);

        if (category == null)
        {
            return NotFound();
        }

        _dbContext.Categories.Remove(category);
        TempData["Message"] = "Category deleted successfully";
        _dbContext.SaveChanges();
        
        return RedirectToAction("Index");
    }
}