using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Helpers;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.UnitOfWork;

namespace WebApplication1.Services
{
    public class ProfilePhotoService : IProfilePhotoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfilePhotoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiResponse<UserDetailsDTO>> CreateUserWithPhotoAsync(UserInputDTOV2 model)
        {
            if (model.FirstName.Any(x => char.IsNumber(x)))
            {
                throw new ValidationException(
                    "Please enter a valid first name. (A-Za-z)",
                    new List<string>
                    {
                        "invalid-firstName"
                    });
            }

            if (model.LastName.Any(x => char.IsNumber(x)))
            {
                throw new ValidationException("Please enter a valid last name. (A-Za-z)", new List<string>
                    {
                        "invalid-lastName"
                    });
            }

            if (await _unitOfWork.Users.AnyAsync(x => x.UserName.Equals(model.UserName)))
            {
                throw new ValidationException(
                    "Duplicate UserName. Please enter another UserName and retry.",
                    new List<string>
                    {
                        "duplicate-UserName"
                    });
            }

            if (await _unitOfWork.Users.AnyAsync(x => x.Email.Equals(model.Email)))
            {
                throw new ValidationException(
                    "Duplicate Email. Please enter another Email and retry.",
                    new List<string>
                    {
                        "duplicate-email"
                    });
            }

            var strategy = _unitOfWork.GetExecutionStrategy();
            var response = await strategy.ExecuteAsync(async () =>
            {
                try
                {
                    await _unitOfWork.BeginTransactionAsync();
                    User user = new User
                    {
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.UserName,
                        Password = model.Password,
                        Address = ""
                    };
                    await _unitOfWork.Users.AddAsync(user);
                    try
                    {
                        await _unitOfWork.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        await _unitOfWork.RollbackAsync();
                        throw new Exception("Something went wrong while saving the data.");
                    }
                    ProfilePhoto profilePhoto = new ProfilePhoto
                    {
                        UserId = user.Id,
                        Url = model.ProfilePhotoUrl
                    };
                    await _unitOfWork.ProfilePhoto.ProfilePhotoAsync(profilePhoto);

                    UserDetailsDTOV2 response = new UserDetailsDTOV2
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        //Address = user.Address,
                        LastName = user.LastName,
                        BirthDate = user.BirthDate
                    };
                    await _unitOfWork.CommitAsync();
                    return new ApiResponse<UserDetailsDTO>
                    {
                        Succeeded = true,
                        Message = "User created with Photo succsffully.",
                        Data = response
                    };
                }
                catch (ValidationException ex)
                {
                    throw new ValidationException(ex.Message, ex.Errors);
                }
            });
            return response;
        }
        public async Task<ApiResponse<UserDetailsDTO>> GetUserDetailsWithPhotoAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ValidationException("Invalid user Id.",
                    new List<string>
                    {
                        "invalid-User Id"
                    });
            }

            var user = await _unitOfWork.Users.GetUserWithPhotoAsync(userId);

            //var users = _dbContext.Users;
            //var usersWihPhoto = users.Include(x => x.ProfilePhoto);
            //var user = await usersWihPhoto.SingleOrDefaultAsync(x => x.Id.Equals(userId));

            //var user = await _dbContext.Users
            //    .SingleOrDefaultAsync(x => x.Id.Equals(userId));

            if (user == null)
            {
                throw new NotFoundException("Could not find any user with the given User Id."
                    ,
                    new List<string>
                    {
                        "Could not find any user with the given User Id."
                    });
            }

            UserDetailsDTOV2 response = new UserDetailsDTOV2
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                // Address = user.Address,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                ProfilePhotoUrl = user.ProfilePhoto?.Url
            };
            return new ApiResponse<UserDetailsDTO>
            {
                Succeeded = true,
                Message = "User Fetched With Photo details succsffully.",
                Data = response
            };
        }
    }
}
