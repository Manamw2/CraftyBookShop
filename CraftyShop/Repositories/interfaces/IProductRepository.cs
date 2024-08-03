using CraftyShop.Models;

namespace CraftyShop.Repositories.interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task Update(Product product);
    }
}
