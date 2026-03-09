using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreWebApplication.Models;
using StoreWebApplication.Models.ViewModels;
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
        IEnumerable<SelectListItem> CategoryList = _unitOfWork.CategoryRepository.GetAll()
            .Select(u=>
                new SelectListItem
                {
                    Text = u.Name, 
                    Value = u.Id.ToString()
                    
                });
        
        return View(objProductList);
    }

    public IActionResult UpSert(int? id)
    {
        IEnumerable<SelectListItem> CategoryList = _unitOfWork.CategoryRepository.GetAll()
            .Select(u=>
                new SelectListItem
                {
                    Text = u.Name, 
                    Value = u.Id.ToString()
                    
                });
        
        ProductVM productVM = new()
        {
            CategoryList = CategoryList,
            Product = new Product()
        };

        if (id == null || id == 0)
        {
            //create
            return View(productVM);
        }
        else
        {
            //update
            productVM.Product = _unitOfWork.ProductRepository.Get(u => u.Id == id);
            return View(productVM);
        }
        
        return View(productVM);
    }
    

    
    [HttpPost]
    public IActionResult Upsert(ProductVM obj, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.ProductRepository.Add(obj.Product);
            _unitOfWork.Save();
            TempData["Message"] = "Product created successfully";
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
        
        ProductVM productVM = new()
        {
            CategoryList =  _unitOfWork.CategoryRepository.GetAll()
                .Select(u=>
                    new SelectListItem
                    {
                        Text = u.Name, 
                        Value = u.Id.ToString()
                    
                    }),
            Product = product
        };
        return View(productVM);
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