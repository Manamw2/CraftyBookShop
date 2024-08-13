using CraftyShop.Data;
using CraftyShop.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
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

        public async Task<T?> Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            if (includeProperties.IsNullOrEmpty())
            {
                return await dbSet.FirstOrDefaultAsync(filter);
            }
            IQueryable<T> query = dbSet;
            foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(prop);
            }
            return await query.FirstOrDefaultAsync(filter);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (!includeProperties.IsNullOrEmpty())
            {
                foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);
                }
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query;
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
