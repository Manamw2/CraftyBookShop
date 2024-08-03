using CraftyShop.Data;
using CraftyShop.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;
namespace CraftyShop.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private ApplicationDbContext _context;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }
        public async Task Add(T entity)
        {
            await dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T?> Get(System.Linq.Expressions.Expression<Func<T, bool>> filter)
        {
            return await dbSet.FirstOrDefaultAsync(filter);
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet;
        }

        public async Task Remove(T entity)
        {
            dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}
