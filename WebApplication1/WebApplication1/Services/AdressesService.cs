
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.DTOs;
using WebApplication1.Exceptions;
using WebApplication1.Helpers;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.UnitOfWork;

namespace WebApplication1.Services
{
    public class AdressesService : IAdressesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdressesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<UserDetailsDTO>> CreateUserWithTwoAdressAsync(UserInputDTOV3 model)
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
                    if (!model.Addresses.IsNullOrEmpty())
                    {
                        ICollection<Address> addressesToInsert = new List<Address>();

                        foreach (var inputAddress in model.Addresses)
                        {
                            Address address = new Address
                            {
                                AddressLine = inputAddress.AddressLine1,
                                City = inputAddress.City,
                                Country = inputAddress.Country,
                                State = inputAddress.State,
                                ZipCode = inputAddress.ZipCode,
                                UserId = user.Id
                            };

                            addressesToInsert.Add(address);
                        }

                        if (addressesToInsert.Count > 0)
                        {
                            await _unitOfWork.Adresses.AddAdressessAsync(addressesToInsert);

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
                        Message = "User created with Two Adresses succsffully.",
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


        public async Task<ApiResponse<UserDetailsDTO>> UpdateUserAddressAsync(int userId, UserAddressUpdateDTO model)
        {
            if (userId <= 0)
            {
                throw new ValidationException("Invalid user Id.",
                    new List<string>
                    {
                        "invalid- user Id"
                    });
            }

            var user = await _unitOfWork.Users.SingleOrDefaultAsync(x => x.Id.Equals(userId));

            if (user == null)
            {
                throw new NotFoundException("Could not find any user with the given User Id.",
                    new List<string>
                    {
                        "Enter Another UserId"
                    });
            }

            user.Address = model.Address;

            var strategy = _unitOfWork.GetExecutionStrategy();
            var response = await strategy.ExecuteAsync(async () =>
            {
                try
                {
                    await _unitOfWork.BeginTransactionAsync();
                    try
                    {
                        await _unitOfWork.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        await _unitOfWork.RollbackAsync();
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
                    await _unitOfWork.CommitAsync();

                    return new ApiResponse<UserDetailsDTO>
                    {
                        Succeeded = true,
                        Message = "User Adress Updated succsfully.",
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
        public async Task<ApiResponse<UserDetailsDTO>> GetUserDetailsWithTwoAdressAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ValidationException("Invalid user Id.",
                    new List<string>
                    {
                        "invalid-User Id"
                    });
            }

            var user = await _unitOfWork.Adresses.GetUserByTwoAdressesAsync(userId);

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

            UserDetailsDTO response = new UserDetailsDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                // Address = user.Address,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                //  ProfilePhotoUrl = user.ProfilePhoto?.Url,
                //addre = user.Address
            };
            return new ApiResponse<UserDetailsDTO>
            {
                Succeeded = true,
                Message = "User Adresses Details Fetched succsffully.",
                Data = response
            };
        }
    }
}
