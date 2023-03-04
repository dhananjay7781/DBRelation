using Microsoft.EntityFrameworkCore;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRolesAsync(IEnumerable<Role> roles)
        {
            await _dbContext.Roles.AddRangeAsync(roles);
        }
        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            return await _dbContext.Roles.SingleOrDefaultAsync(x => x.Id.Equals(roleId));
        }

        public void RemoveUserByRole(Role role)
        {
            _dbContext.Roles.Remove(role);
        }
    }
}
