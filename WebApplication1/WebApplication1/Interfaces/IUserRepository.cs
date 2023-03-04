using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetUserWithPhotoAsync(int userId);
    }
}
