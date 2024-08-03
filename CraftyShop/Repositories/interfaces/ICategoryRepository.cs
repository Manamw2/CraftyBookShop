using CraftyShop.Models;

namespace CraftyShop.Repositories.interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task Update(Category category);
    }
}
