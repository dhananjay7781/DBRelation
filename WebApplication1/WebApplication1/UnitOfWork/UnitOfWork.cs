using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WebApplication1.Exceptions;
using WebApplication1.Interfaces;
using WebApplication1.Repositories;

namespace WebApplication1.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private IDbContextTransaction _objTran;
        private bool disposed = false;

        public IUserRepository Users { get; private set; }

        public IRoleRepository Roles { get; private set; }

        public IUserRoleRepository UserRoles { get; private set; }

        public IProfilePhotoRepository ProfilePhoto { get; private set; }

        public IAdressesRepository Adresses { get; private set; }
        public UnitOfWork(
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            Users = new UserRepository(_dbContext);
            Roles = new RoleRepository(_dbContext);
            UserRoles = new UserRoleRepository(_dbContext);
            ProfilePhoto = new ProfilePhotoRepository(_dbContext);
            Adresses = new AdressesRepository(_dbContext);
        }

        public int SaveChanges()
        {
            try
            {
                return _dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new OperationException(ex.Message, ex.ToString());
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationException(ex.Message, ex.ToString());
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new OperationException(ex.Message, ex.ToString());
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationException(ex.Message, ex.ToString());
            }
        }

        public bool HasChanges()
        {
            return _dbContext.ChangeTracker.HasChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            disposed = true;
        }

        public async Task DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async Task DisposeAsync(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    await _dbContext.DisposeAsync();
                }
            }

            disposed = true;
        }

        public IExecutionStrategy GetExecutionStrategy()
        {
            return _dbContext.Database.CreateExecutionStrategy();
        }

        public void BeginTransaction()
        {
            _objTran = _dbContext.Database.BeginTransaction();
        }

        public async Task BeginTransactionAsync()
        {
            _objTran = await _dbContext.Database.BeginTransactionAsync();
        }

        public void Commit()
        {
            _objTran.Commit();
        }

        public async Task CommitAsync()
        {
            await _objTran.CommitAsync();
        }

        public void Rollback()
        {
            _objTran.Rollback();
            _objTran.Dispose();
        }

        public async Task RollbackAsync()
        {
            await _objTran.RollbackAsync();
            await _objTran.DisposeAsync();
        }
    }
}
