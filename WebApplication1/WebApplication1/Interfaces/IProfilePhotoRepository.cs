using WebApplication1.Models;
using WebApplication1.UnitOfWork;

namespace WebApplication1.Interfaces
{
    public interface IProfilePhotoRepository : IGenericRepository<ProfilePhoto>
    {
        Task ProfilePhotoAsync(ProfilePhoto profilePhoto);
    }
}
