using CraftyShop.Models;

namespace CraftyShop.Repositories.interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task Update(Order order);
        Task UpdateStatus(int Id, string orderStatus, string? paymentStatus);
        Task UpdateStripePaymentId(int id, string sessionId, string stripePaymentId);
    }
}
