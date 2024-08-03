using CraftyShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CraftyShop.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; } = new Product();
        public IEnumerable<SelectListItem> CategoryList { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
