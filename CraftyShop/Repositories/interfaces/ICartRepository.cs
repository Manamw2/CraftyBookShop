using CraftyShop.Models;

namespace CraftyShop.Repositories.interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task Update(Cart cart);
        Task IncerementCount(int cartId);
        Task DecrementCount(int cartId);
    }
}
