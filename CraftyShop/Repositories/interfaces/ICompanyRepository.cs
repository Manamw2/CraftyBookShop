using CraftyShop.Models;

namespace CraftyShop.Repositories.interfaces
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task Update(Company category);
    }
}
