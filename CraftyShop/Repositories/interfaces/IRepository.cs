using System.Linq.Expressions;

namespace CraftyShop.Repositories.interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<T?> Get(Expression<Func<T, bool>> filter);
        Task Add(T entity);
        Task Remove(T entity);
        Task RemoveRange(IEnumerable<T> entities);
    }
}
