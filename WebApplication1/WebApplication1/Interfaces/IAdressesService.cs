using WebApplication1.DTOs;
using WebApplication1.Helpers;

namespace WebApplication1.Interfaces
{
    public interface IAdressesService
    {
        Task<ApiResponse<UserDetailsDTO>> CreateUserWithTwoAdressAsync(UserInputDTOV3 model);

        Task<ApiResponse<UserDetailsDTO>> UpdateUserAddressAsync(int userId, UserAddressUpdateDTO model);
        Task<ApiResponse<UserDetailsDTO>> GetUserDetailsWithTwoAdressAsync(int userId);
    }
}
