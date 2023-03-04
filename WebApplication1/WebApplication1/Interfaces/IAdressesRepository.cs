using WebApplication1.Models;
using WebApplication1.UnitOfWork;

namespace WebApplication1.Interfaces
{
    public interface IAdressesRepository : IGenericRepository<Address>
    {
        Task AddAdressessAsync(IEnumerable<Address> addresses);
        Task<User> GetUserByTwoAdressesAsync(int userId);
    }
}
