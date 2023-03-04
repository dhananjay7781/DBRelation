using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IUserRoleRepository : IGenericRepository<UserRole>
    {
        Task<IEnumerable<RoleOutputDTO>> GetUserRoleByIdAsync(int userId);
    }
}
