using CraftyShop.Data;
using CraftyShop.Models;
using CraftyShop.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CraftyShop.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task Update(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStatus(int Id, string orderStatus, string? paymentStatus)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == Id);
            if (!orderStatus.IsNullOrEmpty())
            {
                order.OrderStatus = orderStatus;
                if (!paymentStatus.IsNullOrEmpty())
                {
                    order.PaymentStatus = paymentStatus;
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateStripePaymentId(int id, string sessionId, string stripePaymentId)
        {
            Order order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (!sessionId.IsNullOrEmpty())
            {
                order.SessionId = sessionId;
            }
            if (!stripePaymentId.IsNullOrEmpty())
            {
                order.PaymentIntentId = stripePaymentId;
                order.PaymentDate = DateTime.Now;
            }
            await _context.SaveChangesAsync();
        }
    }
}
