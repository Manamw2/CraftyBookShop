using CraftyShop.Models;
using CraftyShop.Repositories.interfaces;
using CraftyShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace CraftyShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _ProductRepo;
        private readonly ICategoryRepository _CategoryRepo;
        public ProductController(IProductRepository productRepo, ICategoryRepository categoryRepo)
        {
            _ProductRepo = productRepo;
            _CategoryRepo = categoryRepo;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = _ProductRepo.GetAll().ToList();
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
        public async Task<IActionResult> Upsert(ProductViewModel productVM, FormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _ProductRepo.Add(productVM.Product);
            TempData["success"] = "Product was created successfully";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
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
            IEnumerable<SelectListItem> categoryList = _CategoryRepo.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            ViewBag.CategoryList = categoryList;
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _ProductRepo.Update(product);
            TempData["success"] = "product was Edited successfully";
            return RedirectToAction("Index");
        }

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
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteProduct(int? id)
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
                return View();
            }
            await _ProductRepo.Remove(product);
            TempData["success"] = "Product was deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
