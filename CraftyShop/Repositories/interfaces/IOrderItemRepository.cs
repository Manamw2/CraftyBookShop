using CraftyShop.Models;

namespace CraftyShop.Repositories.interfaces
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        Task Update(OrderItem orderItem);
    }
}
