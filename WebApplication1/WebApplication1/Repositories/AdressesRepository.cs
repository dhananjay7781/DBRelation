using Microsoft.EntityFrameworkCore;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class AdressesRepository : GenericRepository<Address>, IAdressesRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AdressesRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAdressessAsync(IEnumerable<Address> addresses)
        {
            await _dbContext.Addresses.AddRangeAsync(addresses);
        }
        public async Task<User> GetUserByTwoAdressesAsync(int userId)
        {
            return await _dbContext.Users
                .Include(x => x.Addresses)
                .SingleOrDefaultAsync(x => x.Id.Equals(userId));
        }
    }
}
