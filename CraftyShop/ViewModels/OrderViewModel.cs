using CraftyShop.Models;

namespace CraftyShop.ViewModels
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
