using CraftyShop.Data;
using CraftyShop.Models;
using CraftyShop.Repositories.interfaces;
using CraftyShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace CraftyShop.Controllers
{
	public class OrderController : Controller
	{
		private readonly ICartRepository _cartRepo;
		private readonly UserManager<AppUser> _userManager;
		private readonly IOrderRepository _orderRepository;
		private readonly IOrderItemRepository _orderItemRepository;
		[BindProperty]
        public OrderViewModel orderVM { get; set; }
        public OrderController(ICartRepository cartRepo, IOrderRepository orderRepo, IOrderItemRepository orderItemRepository, UserManager<AppUser> userManager)
		{
			_cartRepo = cartRepo;
			_userManager = userManager;
			_orderRepository = orderRepo;
			_orderItemRepository = orderItemRepository;
		}
		public IActionResult Index()
		{
			return View();
		}

		[Authorize]
		public async Task<IActionResult> ConfirmOrder(CartViewModel cartViewModel)
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
			AppUser appUser = await _userManager.FindByIdAsync(userId);
			IEnumerable<Cart> carts = _cartRepo.GetAll(u => u.AppUserId == userId, includeProperties: "Product").ToList();
			cartViewModel.Carts = carts;

			cartViewModel.Order.TotalPrice = carts.Aggregate(0.0, (total, item) => total + (item.Product.Price * item.Count));
			cartViewModel.Order.AppUserId = userId;
			cartViewModel.Order.OrderDate = DateTime.Now;

			if (appUser.CompanyId == null || appUser.CompanyId.GetValueOrDefault() == 0)
			{
				//it is a regular customer 
				cartViewModel.Order.PaymentStatus = SD.PaymentStatusPending;
				cartViewModel.Order.OrderStatus = SD.StatusPending;
			}
			else
			{
				//it is a company user
				cartViewModel.Order.PaymentStatus = SD.PaymentStatusDelayedPayment;
				cartViewModel.Order.OrderStatus = SD.StatusApproved;
			}
			await _orderRepository.Add(cartViewModel.Order);
			foreach (var cart in cartViewModel.Carts)
			{
				OrderItem orderItem = new()
				{
					ProductId = cart.ProductId,
					OrderId = cartViewModel.Order.Id,
					Count = cart.Count
				};
				await _orderItemRepository.Add(orderItem);
			}
			if (appUser.CompanyId == null || appUser.CompanyId.GetValueOrDefault() == 0)
			{
                var domain = "http://localhost:5199/";
                var options = new SessionCreateOptions
                {
                    SuccessUrl = domain + $"Order/OrderConfirmation?id={cartViewModel.Order.Id}",
                    CancelUrl = domain + "Cart/Index",
                    LineItems = new List<SessionLineItemOptions>(),
                
                    Mode = "payment",
                };
                foreach (var item in cartViewModel.Carts)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Product.Price * 100), // $20.50 => 2050
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Title
                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }
                var service = new SessionService();
                Session session = service.Create(options);
				await _orderRepository.UpdateStripePaymentId(cartViewModel.Order.Id, session.Id, session.PaymentIntentId);
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }
            return RedirectToAction("OrderConfirmation", new {id = cartViewModel.Order.Id});
		}

        [Authorize(Roles ="admin, company, employee")]
        [HttpPost]
        public async Task<IActionResult> Details_PAY_NOW()
        {
            orderVM.Order = await _orderRepository.Get(u => u.Id == orderVM.Order.Id, includeProperties: "AppUser");
            orderVM.OrderItems = _orderItemRepository.GetAll(u => u.OrderId == orderVM.Order.Id, includeProperties: "Product");


            //stripe logic
            var domain = "http://localhost:5199/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"Order/OrderConfirmation?id={orderVM.Order.Id}",
                CancelUrl = domain + $"Order/Details?id={orderVM.Order.Id}",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach (var item in orderVM.OrderItems)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100), // $20.50 => 2050
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        }
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineItem);
            }


            var service = new SessionService();
            Session session = service.Create(options);
            await _orderRepository.UpdateStripePaymentId(orderVM.Order.Id, session.Id, session.PaymentIntentId);
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        [Authorize]
        public async Task<IActionResult> OrderConfirmation(int id)
		{
			Order? order = await _orderRepository.Get(u => u.Id == id);
			if (order == null)
			{
				return NotFound();
			}
            
            if (order.PaymentStatus != SD.PaymentStatusDelayedPayment)
			{
                var service = new SessionService();
                Session session = service.Get(order.SessionId);
				if(session.PaymentStatus.ToLower() == "paid")
				{
					await _orderRepository.UpdateStripePaymentId(id, session.Id, session.PaymentIntentId);
					await _orderRepository.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
				}
            }
			IEnumerable<Cart> carts = _cartRepo.GetAll(u => u.AppUserId == order.AppUserId).ToList();
			await _cartRepo.RemoveRange(carts);
            HttpContext.Session.Clear();
            return View(id);
		}

        [Authorize]
		public async Task<IActionResult> Details(int id)
		{
			orderVM = new OrderViewModel();
			Order? order = await _orderRepository.Get(u => u.Id==id, includeProperties:"AppUser");
			IEnumerable<OrderItem> orderItems = _orderItemRepository.GetAll(u => u.OrderId == order.Id, includeProperties: "Product");
			orderVM.Order = order;
			orderVM.OrderItems = orderItems;
			return View(orderVM);
		}
        [Authorize(Roles ="admin, employee")]
        [HttpPost]
		public async Task<IActionResult> UpdateOrder()
		{
			var orderToUpdate = await _orderRepository.Get(u => u.Id == orderVM.Order.Id);
			orderToUpdate.Name = orderVM.Order.Name;
            orderToUpdate.PhoneNumber = orderVM.Order.PhoneNumber;
            orderToUpdate.City = orderVM.Order.City;
            orderToUpdate.State = orderVM.Order.State;
            orderToUpdate.PostalCode = orderVM.Order.PostalCode;
			if (!orderVM.Order.Carrier.IsNullOrEmpty())
			{
				orderToUpdate.Carrier = orderVM.Order.Carrier;
			}
            if (!orderVM.Order.TrackingNumber.IsNullOrEmpty())
            {
                orderToUpdate.Carrier = orderVM.Order.TrackingNumber;
            }
            await _orderRepository.Update(orderToUpdate);
			TempData["success"] = "Order Details were updated successfully";
			return RedirectToAction("Details", new {id = orderVM.Order.Id });
		}

        [Authorize(Roles ="admin, employee")]
        public async Task<IActionResult> StartProcessing()
        {
            await _orderRepository.UpdateStatus(orderVM.Order.Id, SD.StatusInProcess, null);
            TempData["success"] = "Order is processing successfully";
            return RedirectToAction("Details", new { id = orderVM.Order.Id });
        }

        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> ShipOrder()
        {
            var orderToUpdate = await _orderRepository.Get(u => u.Id == orderVM.Order.Id);
			orderToUpdate.Carrier = orderVM.Order.Carrier;
			orderToUpdate.TrackingNumber = orderVM.Order.TrackingNumber;
			orderToUpdate.OrderStatus = SD.StatusShipped;
			orderToUpdate.ShippingDate = DateTime.Now;
			if(orderToUpdate.PaymentStatus == SD.PaymentStatusDelayedPayment)
			{
				orderToUpdate.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
			}
			await _orderRepository.Update(orderToUpdate);
            TempData["success"] = "Order is Shipped successfully";
            return RedirectToAction("Details", new { id = orderVM.Order.Id });
        }

        [Authorize(Roles = "admin, employee")]
        public async Task<IActionResult> CancelOrder()
		{
            var order = await _orderRepository.Get(u => u.Id == orderVM.Order.Id);
			if(order.PaymentStatus == SD.PaymentStatusApproved)
			{
				var options = new RefundCreateOptions
				{
					Reason = RefundReasons.RequestedByCustomer,
					PaymentIntent = order.PaymentIntentId,
				};
				var service = new RefundService();
				Refund refund = await service.CreateAsync(options);
				await _orderRepository.UpdateStatus(order.Id, SD.StatusCancelled, SD.StatusRefunded);
			}
			else
			{
                await _orderRepository.UpdateStatus(order.Id, SD.StatusCancelled, SD.StatusCancelled);
            }
            TempData["success"] = "Order is Cancelled successfully";
            return RedirectToAction("Details", new { id = orderVM.Order.Id });
        }

        #region API Calls
        [HttpGet]
		public IActionResult GetAll(string status)
		{
            if(User.IsInRole("admin") || User.IsInRole("employee"))
			{
                IEnumerable<Order> orders = _orderRepository.GetAll(includeProperties: "AppUser");
                if (status != "all")
                {
                    orders = orders.Where(u => u.OrderStatus == status);
                }

                return Json(new { data = orders.ToList() });
            }
			else
			{
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                IEnumerable<Order> orders = _orderRepository.GetAll(u => u.AppUserId == userId, includeProperties: "AppUser");
                if (status != "all")
                {
                    orders = orders.Where(u => u.OrderStatus == status);
                }
                return Json(new { data = orders.ToList() });
            }
		}
        #endregion
    }
}
