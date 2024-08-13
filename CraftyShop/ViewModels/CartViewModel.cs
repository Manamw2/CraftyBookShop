using CraftyShop.Models;

namespace CraftyShop.ViewModels
{
    public class CartViewModel
    {
        public IEnumerable<Cart> Carts { get; set; }
        public Order? Order { get; set; }
        public double Total { get; set; }
    }
}
