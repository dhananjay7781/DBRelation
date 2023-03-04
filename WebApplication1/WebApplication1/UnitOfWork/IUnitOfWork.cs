using Microsoft.EntityFrameworkCore.Storage;
using WebApplication1.Interfaces;

namespace WebApplication1.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // Methods
        int SaveChanges();

        Task<int> SaveChangesAsync();

        bool HasChanges();

        void Dispose();

        Task DisposeAsync();

        IExecutionStrategy GetExecutionStrategy();

        void BeginTransaction();

        Task BeginTransactionAsync();

        void Commit();

        Task CommitAsync();

        void Rollback();

        Task RollbackAsync();

        // Interfaces
        IUserRepository Users { get; }

        IRoleRepository Roles { get; }

        IUserRoleRepository UserRoles { get; }

        IProfilePhotoRepository ProfilePhoto { get; }

        IAdressesRepository Adresses { get; }
    }
}
