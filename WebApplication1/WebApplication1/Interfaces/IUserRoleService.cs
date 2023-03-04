using WebApplication1.DTOs;
using WebApplication1.Helpers;

namespace WebApplication1.Interfaces
{
    public interface IUserRoleService
    {
        Task<ApiResponse<UserDetailsDTO>> GetUserRolesAsync(int userId);
    }
}
