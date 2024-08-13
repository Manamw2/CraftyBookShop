using CraftyShop.Data;
using CraftyShop.Models;
using CraftyShop.Repositories.interfaces;

namespace CraftyShop.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _context;
        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task Update(Company category)
        {
            _context.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
