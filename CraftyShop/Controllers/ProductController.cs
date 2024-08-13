using CraftyShop.Models;
using CraftyShop.Repositories.interfaces;
using CraftyShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace CraftyShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _ProductRepo;
        private readonly ICategoryRepository _CategoryRepo;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProductController(IProductRepository productRepo, ICategoryRepository categoryRepo, IWebHostEnvironment webHostEnvironment)
        {
            _ProductRepo = productRepo;
            _CategoryRepo = categoryRepo;
            _WebHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = _ProductRepo.GetAll(includeProperties: "Category").ToList();
            return View(products);
        }
        public async Task<IActionResult> Upsert(int? id)
        {

            IEnumerable<SelectListItem> categoryList = _CategoryRepo.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            ProductViewModel productVM = new ProductViewModel
            {
                CategoryList = categoryList
            };
            if(id != null)
            {
                Product? product = await _ProductRepo.Get(x => x.Id == id.Value);
                if (product == null)
                {
                    return NotFound();
                }
                productVM.Product = product;
            }
            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(ProductViewModel productVM, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                productVM.CategoryList = _CategoryRepo.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
            string wwwRootPath = _WebHostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\product");

                if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                {
                    //Delete Old Image 
                    var oldImagePath =
                        Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                    if (!oldImagePath.IsNullOrEmpty() && System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                productVM.Product.ImageUrl = @"\images\product\" + fileName;
            }

            if(productVM.Product.Id == 0)
            {
                await _ProductRepo.Add(productVM.Product);
                TempData["success"] = "Product was created successfully";
            }
            else
            {
                await _ProductRepo.Update(productVM.Product);
                TempData["success"] = "Product was updated successfully";
            }
            return RedirectToAction("Index");
        }
        
        #region Api Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = _ProductRepo.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = products });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product? product = await _ProductRepo.Get(x => x.Id == id.Value);
            if (product == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _ProductRepo.Remove(product);

            string wwwRootPath = _WebHostEnvironment.WebRootPath;
            //Delete Old Image 
            var oldImagePath =
                Path.Combine(wwwRootPath, product.ImageUrl.TrimStart('\\'));

            if (!oldImagePath.IsNullOrEmpty() && System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            return NoContent();
        }
        #endregion
    }
}
