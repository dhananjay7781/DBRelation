using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.DTOs;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.UnitOfWork;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IProfilePhotoRepository _profilePhotoRepository;
        private readonly IAdressesRepository _adressesRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserService _userService;
        private readonly IProfilePhotoService _profilePhotoService;
        private readonly IAdressesService _adressesService;
        private readonly IRolesService _rolesService;
        private readonly IUserRoleService _userRoleService;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(
            ILogger<UsersController> logger,
            IUserRepository userRepository,
            IProfilePhotoRepository profilePhotoRepository,
            IAdressesRepository adressesRepository,
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository,
            IUserService userService,
            IProfilePhotoService profilePhotoService,
            IAdressesService adressesService,
            IRolesService rolesService,
            IUserRoleService userRoleService,
            IUnitOfWork unitOfWork
            )
        {
            _logger = logger;
            _userRepository = userRepository;
            _profilePhotoRepository = profilePhotoRepository;
            _adressesRepository = adressesRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _userService = userService;
            _profilePhotoService = profilePhotoService;
            _adressesService = adressesService;
            _rolesService = rolesService;
            _userRoleService = userRoleService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUserAsync(UserInputDTO model)
        {
            return Ok(await _userService.CreateUserAsync(model));
        }

        [HttpPost("createWithPhoto")]
        public async Task<IActionResult> CreateUserWithPhotoAsync(UserInputDTOV2 model)
        {
            return Ok(await _profilePhotoService.CreateUserWithPhotoAsync(model));
        }

        //Creating user with two adrrsses

        [HttpPost("createWithAdresses")]
        public async Task<IActionResult> CreateUserWithTwoAdressAsync(UserInputDTOV3 model)
        {
            return Ok(_adressesService.CreateUserWithTwoAdressAsync(model));
        }

        //creating user with multiple roles and vice-versa

        [HttpPost("createWithRoles")]
        public async Task<IActionResult> CreateUserWithRolesAsync(UserInputDTOV4 model)
        {
            return Ok(await _rolesService.CreateUserWithRolesAsync(model));
        }


        /* [HttpPost("createWithMultipleRoles")]
         public async Task<IActionResult> CreateUserWithMultipleRoleAsync(UserInputDTOV4 model)
         {
             return Ok(await _rolesService.CreateUserWithMultipleRoleAsync(model));
         }*/


        [HttpPatch("{userId}/updateAddress")]
        public async Task<IActionResult> UpdateUserAddressAsync(int userId, UserAddressUpdateDTO model)
        {

            return Ok(await _adressesService.UpdateUserAddressAsync(userId, model));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserDetailsAsync(int userId)
        {




            return Ok(await _userService.GetUserDetailsAsync(userId));
        }

        [HttpGet("getWithPhoto/{userId}")]
        public async Task<IActionResult> GetUserDetailsWithPhotoAsync(int userId)
        {
            return Ok(await _profilePhotoService.GetUserDetailsWithPhotoAsync(userId));
        }

        //get user with two adresses

        [HttpGet("getWithTwoAdress/{userId}")]
        public async Task<IActionResult> GetUserDetailsWithTwoAdressAsync(int userId)
        {
            return Ok(await _adressesService.GetUserDetailsWithTwoAdressAsync(userId));
        }

        //To Delete user from databse
        [HttpDelete("{userId}")]
        public async Task<IActionResult> RemoveUserAsync(int userId)
        {
            //return Ok($"User with Id {userId} removed.");
            return Ok(await _userService.RemoveUserAsync(userId));
        }



        //to delete role from database
        [HttpDelete("removeRole/{roleId}")]
        public async Task<IActionResult> RemoveRoleAsync(int roleId)
        {
            /* if (roleId <= 0)
             {
                 return BadRequest("Invalid Role Id.");
             }

             //using var db = new ApplicationDbContext();

             var user = await _roleRepository.GetRoleByIdAsync(roleId);
             if (user == null)
             {
                 return NotFound("Could not find any Role with the given Role Id.");
             }

             _roleRepository.RemoveUserByRole(user);

             try
             {
                 await _userRepository.SaveChangesAsync();
             }
             catch (Exception ex)
             {
                 _logger.LogError(ex.Message);
                 _logger.LogError(ex.StackTrace);
                 return StatusCode(500, ex.Message);
             }*/

            // return Ok($"Role with Id {roleId} removed.");
            return Ok(await _rolesService.RemoveRoleAsync(roleId));
        }
        /////

        [HttpGet("{userId}/roles")]
        public async Task<IActionResult> GetUserRolesAsync(int userId)
        {
            /*if (userId <= 0)
            {
                return BadRequest("Invalid user Id.");
            }

            if (!await _userRepository.AnyAsync(x => x.Id.Equals(userId)))
            {
                return NotFound("No user found in the database with the given user Id.");
            }

            var roles = await _userRoleRepository.GetUserRoleByIdAsync(userId);*/

            return Ok(await _userRoleService.GetUserRolesAsync(userId));
        }
    }
}
