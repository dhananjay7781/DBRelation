using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
    public class RolesService : IRolesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolesService(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<UserDetailsDTO>> CreateUserWithRolesAsync(UserInputDTOV4 model)
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
                    if (!model.Roles.IsNullOrEmpty())
                    {
                        ICollection<Role> rolesToInsert = new List<Role>();

                        foreach (var inputRoles in model.Roles)
                        {
                            Role role = new Role
                            {
                                RoleName = inputRoles.RoleName,
                            };

                            rolesToInsert.Add(role);
                        }

                        if (rolesToInsert.Count > 0)
                        {
                            await _unitOfWork.Roles.AddRolesAsync(rolesToInsert);

                            try
                            {
                                await _unitOfWork.SaveChangesAsync();
                            }
                            catch (Exception ex)
                            {
                                await _unitOfWork.RollbackAsync();
                                throw new Exception("Something went wrong while saving the data.");
                            }
                        }
                    }
                    UserDetailsDTO response = new UserDetailsDTO
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        BirthDate = user.BirthDate
                    };

                    await _unitOfWork.CommitAsync();
                    return new ApiResponse<UserDetailsDTO>
                    {
                        Succeeded = true,
                        Message = "User created with Role succsffully.",
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

        /*public async Task<ApiResponse<UserDetailsDTO>> CreateUserWithMultipleRoleAsync(UserInputDTOV4 model)
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

            if (await _userRepository.AnyAsync(x => x.UserName.Equals(model.UserName)))
            {
                throw new ValidationException(
                    "Duplicate UserName. Please enter another UserName and retry.",
                    new List<string>
                    {
                        "duplicate-UserName"
                    });
            }

            if (await _userRepository.AnyAsync(x => x.Email.Equals(model.Email)))
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
            };
            await _userRepository.AddAsync(user);
            if (await _userRepository.SaveChangesAsync() <= 0)
            {
                throw new Exception("Something went wrong while saving the data.");
            }
            if (!model.Roles.IsNullOrEmpty())
            {
                ICollection<Role> rolesToInsert = new List<Role>();

                foreach (var inputRoles in model.Roles)
                {

                    Role role = new Role
                    {
                        RoleName = inputRoles.RoleName
                    };

                    rolesToInsert.Add(role);
                }

                if (rolesToInsert.Count > 0)
                {
                    await _roleRepository.AddRolesAsync(rolesToInsert);

                    if (await _userRepository.SaveChangesAsync() <= 0)
                    {
                        throw new Exception("Something went wrong while saving the data.");
                    }
                }
            }
            UserDetailsDTO response = new UserDetailsDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate
            };
            return new ApiResponse<UserDetailsDTO>
            {
                Succeeded = true,
                Message = "User created With Multiple Roles succsffully.",
                Data = response
            };
        }*/

        //Remove Roles
        public async Task<ApiResponse<UserDetailsDTO>> RemoveRoleAsync(int roleId)
        {
            if (roleId <= 0)
            {
                throw new ValidationException("Invalid role Id.",
                    new List<string>
                    {
                        "invalid-role Id"
                    });
            }

            //using var db = new ApplicationDbContext();




            var user = await _unitOfWork.Roles.GetRoleByIdAsync(roleId);
            if (user == null)
            {
                throw new NotFoundException("Could not find any user with the given User Id."
                    ,
                    new List<string>
                    {
                        "Could not find any user with the given User Id."
                    });
            }

            _unitOfWork.Roles.RemoveUserByRole(user);

            if (await _unitOfWork.SaveChangesAsync() <= 0)
            {
                throw new Exception("Something went wrong while saving the data.");
            }

            return new ApiResponse<UserDetailsDTO>
            {
                Succeeded = true,
                Message = "Role Removed succsffully.",
                Data = user
            };
        }
    }
}
