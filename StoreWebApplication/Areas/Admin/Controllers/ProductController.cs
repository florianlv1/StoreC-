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
    private readonly IWebHostEnvironment _webHostEnvironment;
    
    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }
    
    public IActionResult Index()
    {

        List<Product> objProductList = _unitOfWork.ProductRepository.GetAll(includeProperties:"Category").ToList();
        
        
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

        if (id != null || id != 0)
        {
            //update
            productVM.Product = _unitOfWork.ProductRepository.Get(u => u.Id == id);
        }
        
        return View(productVM);
    }
    

    
    [HttpPost]
    public IActionResult Upsert(ProductVM obj, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images/product");

                if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
                {
                    //delete old image
                    var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                } ;
                
                obj.Product.ImageUrl = @"images/product/" + fileName;
            }

            if (obj.Product.Id == 0)
            {
                _unitOfWork.ProductRepository.Add(obj.Product);
                TempData["Message"] = "Product created successfully";
            }
            else
            {
                _unitOfWork.ProductRepository.Update(obj.Product);
                TempData["Message"] = "Product updated successfully";
            }
            
            _unitOfWork.Save();
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