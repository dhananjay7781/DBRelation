using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Helpers;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.UnitOfWork;

namespace WebApplication1.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<UserDetailsDTO>> CreateUserAsync(UserInputDTO model)
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
            if (await _unitOfWork.SaveChangesAsync() <= 0)
            {
                throw new Exception("Something went wrong while saving the data.");
            }

            UserDetailsDTO response = new UserDetailsDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                //Address = user.Address,
                LastName = user.LastName,
                BirthDate = user.BirthDate
            };

            return new ApiResponse<UserDetailsDTO>
            {
                Succeeded = true,
                Message = "User created succsffully.",
                Data = response
            };
        }

        public async Task<ApiResponse<UserDetailsDTO>> GetUserDetailsAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ValidationException("Invalid user Id.",
                    new List<string>
                    {
                        "invalid-User Id"
                    });
            }

            var user = await _unitOfWork.Users.SingleOrDefaultAsync(x => x.Id.Equals(userId));
            if (user == null)
            {
                throw new NotFoundException("Could not find any user with the given User Id."
                    ,
                    new List<string>
                    {
                        "Could not find any user with the given User Id."
                    });
            }

            UserDetailsDTO response = new UserDetailsDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                //Address = user.Address,
                LastName = user.LastName,
                BirthDate = user.BirthDate
            };
            return new ApiResponse<UserDetailsDTO>
            {
                Succeeded = true,
                Message = "User Details Fetched succsffully.",
                Data = response
            };
        }

        public async Task<ApiResponse<UserDetailsDTO>> RemoveUserAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ValidationException("Invalid user Id.",
                    new List<string>
                    {
                        "invalid-User Id"
                    });
            }

            //using var db = new ApplicationDbContext();

            var user = await _unitOfWork.Users.SingleOrDefaultAsync(x => x.Id.Equals(userId));
            if (user == null)
            {
                throw new NotFoundException("Could not find any user with the given User Id."
                    ,
                    new List<string>
                    {
                        "Could not find any user with the given User Id."
                    });
            }

            _unitOfWork.Users.Remove(user);

            if (await _unitOfWork.SaveChangesAsync() <= 0)
            {
                throw new Exception("Something went wrong while saving the data.");
            }
            return new ApiResponse<UserDetailsDTO>
            {
                Succeeded = true,
                Message = "User Removed succsffully."
            };
        }

        public async Task<ApiResponse<UserDetailsDTOV3>> CreateUserWithRoleAsync(UserInputDTO model)
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
                        throw new OperationException("Something went wrong while saving the data.", "record-save-error");
                    }

                    var role = await _unitOfWork.Roles.SingleOrDefaultAsync(x => x.RoleName.Equals("USER"));
                    if (role == null)
                    {
                        await _unitOfWork.RollbackAsync();
                        throw new NotFoundException("Could not find USER role in the databse", "USER-Role-NotFound");
                    }

                    UserRole userRole = new UserRole
                    {
                        UserId = user.Id,
                        RoleId = role.Id,
                    };

                    try
                    {
                        await _unitOfWork.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        await _unitOfWork.RollbackAsync();
                        throw new OperationException("Something went wrong while saving the data.", "record-save-error");
                    }

                    UserDetailsDTOV3 response = new UserDetailsDTOV3
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Roles = new List<string>
                        {
                            role.RoleName,
                        }
                    };

                    await _unitOfWork.CommitAsync();

                    return new ApiResponse<UserDetailsDTOV3>
                    {
                        Succeeded = true,
                        Message = "User created succsffully.",
                        Data = response
                    };
                }
                catch (ValidationException ex)
                {
                    throw new ValidationException(ex.Message, ex.Errors);
                }
                catch (NotFoundException ex)
                {
                    throw new NotFoundException(ex.Message, ex.Errors);
                }
                catch (OperationException ex)
                {
                    throw new OperationException(ex.Message, ex.Errors);
                }
            });

            return response;
        }
    }
}
