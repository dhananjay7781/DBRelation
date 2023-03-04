using WebApplication1.DTOs;
using WebApplication1.Helpers;

namespace WebApplication1.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<UserDetailsDTO>> CreateUserAsync(UserInputDTO model);
        Task<ApiResponse<UserDetailsDTO>> GetUserDetailsAsync(int userId);
        Task<ApiResponse<UserDetailsDTO>> RemoveUserAsync(int userId);
        Task<ApiResponse<UserDetailsDTOV3>> CreateUserWithRoleAsync(UserInputDTO model);
    }
}
