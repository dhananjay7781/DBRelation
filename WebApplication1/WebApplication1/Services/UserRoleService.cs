using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Helpers;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.UnitOfWork;

namespace WebApplication1.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRoleService(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiResponse<UserDetailsDTO>> GetUserRolesAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ValidationException("Invalid user Id.",
                    new List<string>
                    {
                        "invalid-User Id"
                    });
            }

            /*if (user == null)
            {
                throw new NotFoundException("Could not find any user with the given User Id."
                    ,
                    new List<string>
                    {
                        "Could not find any user with the given User Id."
                    });
            }*/

            var roles = await _unitOfWork.UserRoles.GetUserRoleByIdAsync(userId);
            return new ApiResponse<UserDetailsDTO>
            {
                Succeeded = true,
                Message = "UserRole Fetched succsffully.",
                Data = (UserDetailsDTO)roles
            };
        }
    }
}
