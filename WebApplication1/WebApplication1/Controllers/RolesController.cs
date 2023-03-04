using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<RolesController> _logger;

        public RolesController(
            ApplicationDbContext dbContext,
            ILogger<RolesController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRoleAsync(RoleInputDTO model)
        {
            string normalizedRoleName = model.RoleName.Trim().ToUpperInvariant();
            normalizedRoleName = normalizedRoleName.Replace(" ", "");

            if (await _dbContext.Roles.AnyAsync(x => x.RoleName.Equals(normalizedRoleName)))
            {
                return BadRequest("Role already exists in the database.");
            }

            Role role = new Role
            {
                RoleName = normalizedRoleName
            };

            await _dbContext.Roles.AddAsync(role);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                return StatusCode(500, ex.Message);
            }

            return Ok("Role created successfully.");
        }

        [HttpPatch("updateUserRole")]
        public async Task<IActionResult> UpdateSingleUserRoleAsync(UserRoleAssociationDTO model)
        {
            if (model.UserId <= 0)
            {
                return BadRequest("Invalid user Id.");
            }

            if (model.RoleId <= 0)
            {
                return BadRequest("Invalid role Id.");
            }

            if (await _dbContext.UserRoles.AnyAsync(x => x.UserId.Equals(model.UserId) && x.RoleId.Equals(model.RoleId)))
            {
                return BadRequest("User-Role association already exists in the database.");
            }

            UserRole userRole = new UserRole
            {
                UserId = model.UserId,
                RoleId = model.RoleId
            };

            await _dbContext.UserRoles.AddAsync(userRole);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                return StatusCode(500, ex.Message);
            }

            return Ok("User-Role association created successfully.");
        }
    }
}
