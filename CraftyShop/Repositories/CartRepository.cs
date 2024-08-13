using CraftyShop.Data;
using CraftyShop.Models;
using CraftyShop.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace CraftyShop.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task IncerementCount(int cartId)
        {
            Cart? cart = await _context.Carts.FirstOrDefaultAsync(u => u.Id == cartId);
            if (cart != null)
            {
                if (cart.Count < int.MaxValue)
                {
                    cart.Count = cart.Count + 1;
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task DecrementCount(int cartId)
        {
            Cart? cart = await _context.Carts.FirstOrDefaultAsync(u => u.Id == cartId);
            if (cart != null)
            {
                if (cart.Count > 0)
                {
                    cart.Count = cart.Count - 1;
                }
                else {
                    _context.Carts.Remove(cart);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task Update(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }
    }
}
