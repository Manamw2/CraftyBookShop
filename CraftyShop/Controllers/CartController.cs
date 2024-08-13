using CraftyShop.Models;
using CraftyShop.Data;
using CraftyShop.Repositories.interfaces;
using CraftyShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CraftyShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepo;
        private readonly UserManager<AppUser> _userManager;
        private CartViewModel cartVM { get; set; }
        public CartController(ICartRepository cartRepo, UserManager<AppUser> userManager)
        {
            _cartRepo = cartRepo;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser appUser = await _userManager.FindByIdAsync(userId);
            IEnumerable<Cart> carts = _cartRepo.GetAll(u => u.AppUserId == userId, includeProperties: "Product");
            double total = carts.Aggregate(0.0, (total, item) => total + (item.Product.Price * item.Count));
            cartVM = new CartViewModel
            {
                Carts = carts,
                Total = total,
            };
            return View(cartVM);
        }
        
        public async Task<IActionResult> Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser appUser = await _userManager.FindByIdAsync(userId);
            IEnumerable<Cart> carts = _cartRepo.GetAll(u => u.AppUserId == userId, includeProperties: "Product");
            double total = carts.Aggregate(0.0, (total, item) => total + (item.Product.Price * item.Count));
            Order order = new Order
            {
                AppUserId = userId,
                Name = appUser.UserName,
                PhoneNumber = appUser.PhoneNumber,
                City = appUser.City,
                State = appUser.State,
                StreetAddress = appUser.StreetAddress,
                PostalCode = appUser.PostalCode,
                TotalPrice = total,
            };
            cartVM = new()
            {
                Carts = carts,
                Order = order,
                Total = total,
            };
            return View(cartVM);
        }

            public async Task<IActionResult> IncrementCount(int id)
        {
            await _cartRepo.IncerementCount(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DecrementCount(int id)
        {
            await _cartRepo.DecrementCount(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            Cart? cart = await _cartRepo.Get(u => u.Id == id);
            if (cart == null)
            {
                return RedirectToAction("Index");
            }
            await _cartRepo.Remove(cart);
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            int userCartsCount = _cartRepo.GetAll(u => u.AppUserId == userId).Count();
            HttpContext.Session.SetInt32(SD.SessionCart, userCartsCount);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddCart(int id, int count)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            Cart? cart = await _cartRepo.Get(u => u.ProductId == id && u.AppUserId == userId);
            if (cart == null)
            {
                Cart newCart = new Cart
                {
                    ProductId = id,
                    Count = count,
                    AppUserId = userId,
                };
                await _cartRepo.Add(newCart);
            }
            else
            {
                cart.Count = count;
                await _cartRepo.Update(cart);
            }
            int userCartsCount = _cartRepo.GetAll(u => u.AppUserId == userId).Count();
            HttpContext.Session.SetInt32(SD.SessionCart, userCartsCount);
            TempData["success"] = "Product was added to cart successfully";
            return RedirectToAction("Index", "Home");
        }
    }
}
