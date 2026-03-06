using Microsoft.AspNetCore.Mvc;
using StoreWebApplication.Models;
using StoreWebApplication.Repository.IRepository;

namespace StoreWebApplication.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        List<Product> objProductList = _unitOfWork.ProductRepository.GetAll().ToList();
        
        return View(objProductList);
    }

    public IActionResult Create()
    {
        return View();
    }
    

    
    [HttpPost]
    public IActionResult Create(Product product)
    {
        if (product.Title == "1234")
        {
            ModelState.AddModelError("Title", "The Title cannot be 1234");
        }

        if (ModelState.IsValid)
        {
            _unitOfWork.ProductRepository.Add(product);
            _unitOfWork.Save();
            TempData["Message"] = "Product created successfully";
            return RedirectToAction("Index", "Product");

        }

        return View();
    }
    
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound(); 
        }
        Product product = _unitOfWork.ProductRepository.Get(u=>u.Id==id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }
    
    [HttpPost]
    public IActionResult Edit(Product product)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.ProductRepository.Update(product);
            _unitOfWork.Save();
            TempData["Message"] = "Product updated successfully";
            return RedirectToAction("Index", "Product");

        }

        return View();
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        Product product = _unitOfWork.ProductRepository.Get(u=>u.Id==id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? id)
    {
        Product product = _unitOfWork.ProductRepository.Get(u => u.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        _unitOfWork.ProductRepository.Remove(product);
        TempData["Message"] = "Product deleted successfully";
        _unitOfWork.Save();
        
        return RedirectToAction("Index");
    }
}