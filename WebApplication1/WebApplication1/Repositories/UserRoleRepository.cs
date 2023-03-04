using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRoleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<RoleOutputDTO>> GetUserRoleByIdAsync(int userId)
        {
            return await _dbContext.UserRoles
                .Where(x => x.UserId.Equals(userId))
                .Join(_dbContext.Roles,
                userRole => userRole.RoleId,
                role => role.Id,
                (userRole, role) => new RoleOutputDTO
                {
                    RoleId = role.Id,
                    RoleName = role.RoleName
                })
                .ToListAsync();
        }
    }
}
