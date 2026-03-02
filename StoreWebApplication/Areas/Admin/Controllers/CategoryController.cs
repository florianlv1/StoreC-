using Microsoft.AspNetCore.Mvc;
using StoreWebApplication.Models;
using StoreWebApplication.Repository.IRepository;

namespace StoreWebApplication.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        List<Category> objCategoryList = _unitOfWork.CategoryRepository.GetAll().ToList();
        
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
            _unitOfWork.CategoryRepository.Add(category);
            _unitOfWork.Save();
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
        Category category = _unitOfWork.CategoryRepository.Get(u=>u.Id==id);
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
            _unitOfWork.CategoryRepository.Update(category);
            _unitOfWork.Save();
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

        Category category = _unitOfWork.CategoryRepository.Get(u=>u.Id==id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? id)
    {
        Category category = _unitOfWork.CategoryRepository.Get(u => u.Id == id);

        if (category == null)
        {
            return NotFound();
        }

        _unitOfWork.CategoryRepository.Remove(category);
        TempData["Message"] = "Category deleted successfully";
        _unitOfWork.Save();
        
        return RedirectToAction("Index");
    }
}