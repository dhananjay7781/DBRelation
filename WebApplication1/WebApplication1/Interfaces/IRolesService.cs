using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Helpers;

namespace WebApplication1.Interfaces
{
    public interface IRolesService
    {
        Task<ApiResponse<UserDetailsDTO>> CreateUserWithRolesAsync(UserInputDTOV4 model);
       // Task<ApiResponse<UserDetailsDTO>> CreateUserWithMultipleRoleAsync(UserInputDTOV4 model);
        Task<ApiResponse<UserDetailsDTO>> RemoveRoleAsync(int roleId);
    }
}
