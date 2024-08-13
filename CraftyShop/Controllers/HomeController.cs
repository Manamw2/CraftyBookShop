using CraftyShop.Models;
using CraftyShop.Repositories.interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CraftyShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepo;

        public HomeController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product? product = await _productRepo.Get(x => x.Id == id.Value, "Category");
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        public IActionResult Index()
        {
            IEnumerable<Product> products = _productRepo.GetAll(includeProperties: "Category");
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
