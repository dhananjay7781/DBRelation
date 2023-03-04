using Microsoft.EntityFrameworkCore;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserWithPhotoAsync(int userId)
        {
            return await _dbContext.Users
                .Include(x => x.ProfilePhoto)
                .SingleOrDefaultAsync(x => x.Id.Equals(userId));
        }
    }
}
