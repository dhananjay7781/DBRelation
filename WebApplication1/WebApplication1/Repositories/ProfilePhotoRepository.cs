using Microsoft.EntityFrameworkCore;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class ProfilePhotoRepository : GenericRepository<ProfilePhoto> ,IProfilePhotoRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProfilePhotoRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task ProfilePhotoAsync(ProfilePhoto profilePhoto)
        {
            await _dbContext.ProfilePhotos.AddAsync(profilePhoto);
        }
    }
}
