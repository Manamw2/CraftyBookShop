using CraftyShop.Data;
using CraftyShop.Models;
using CraftyShop.Repositories.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CraftyShop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepo = categoryRepository;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> categories = _categoryRepo.GetAll().ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category cat)
        {
            if (!cat.Name.IsNullOrEmpty() && cat.Name.ToLower().Trim() == "porn")
            {
                ModelState.AddModelError("Name", "Invalid Category Name");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _categoryRepo.Add(cat);
            TempData["success"] = "Categories was created successfully";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category? cat = await _categoryRepo.Get(x => x.Id == id.Value);
            if (cat == null)
            {
                return NotFound();
            }
            return View(cat);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category cat)
        {
            if (!cat.Name.IsNullOrEmpty() && cat.Name.ToLower().Trim() == "porn")
            {
                ModelState.AddModelError("Name", "Invalid Category Name");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _categoryRepo.Update(cat);
            TempData["success"] = "Categories was Edited successfully";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category? cat = await _categoryRepo.Get(x => x.Id == id.Value);
            if (cat == null)
            {
                return NotFound();
            }
            return View(cat);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category? cat = await _categoryRepo.Get(x => x.Id == id.Value);
            if (cat == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _categoryRepo.Remove(cat);
            TempData["success"] = "Categories was deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
