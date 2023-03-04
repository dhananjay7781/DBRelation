using WebApplication1.DTOs;
using WebApplication1.Helpers;

namespace WebApplication1.Interfaces
{
    public interface IProfilePhotoService
    {
        Task<ApiResponse<UserDetailsDTO>> CreateUserWithPhotoAsync(UserInputDTOV2 model);
        Task<ApiResponse<UserDetailsDTO>> GetUserDetailsWithPhotoAsync(int userId);
    }
}
