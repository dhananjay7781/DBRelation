using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task AddRolesAsync(IEnumerable<Role> roles);

        Task<Role> GetRoleByIdAsync(int roleId);

        void RemoveUserByRole(Role role);
    }

}
