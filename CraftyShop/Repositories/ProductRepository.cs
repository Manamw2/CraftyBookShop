using CraftyShop.Data;
using CraftyShop.Models;
using CraftyShop.Repositories.interfaces;

namespace CraftyShop.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task Update(Product product)
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
