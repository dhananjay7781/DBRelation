using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApplication1.Interfaces;

namespace WebApplication1.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().AddRange(entities);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
        }

        public int Count(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>().Count(expression);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().CountAsync(expression);
        }

        public void Remove(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>().Where(expression).ToList();
        }

        public async Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().Where(expression).ToListAsync();
        }

        public bool Any(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>().Any(expression);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().AnyAsync(expression);
        }

        public T? SingleOrDefault(Expression<Func<T, bool>> expression) 
        {
            return _dbContext.Set<T>().SingleOrDefault(expression);
        }

        public async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().SingleOrDefaultAsync(expression);
        }

        public T? FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>().FirstOrDefault(expression);
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(expression);
        }

        public T? Find(Expression<Func<T, bool>> expression)
        {
            return _dbContext.Set<T>().Find(expression);
        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().FindAsync(expression);
        }

        public IEnumerable<T> ToList()
        {
            return _dbContext.Set<T>().ToList();
        }

        public async Task<IEnumerable<T>> ToListAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
    }
}
