using System.Linq.Expressions;

namespace WebApplication1.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        void Add(T entity);

        Task AddAsync(T entity);

        void AddRange(IEnumerable<T> entities);

        Task AddRangeAsync(IEnumerable<T> entities);

        int Count(Expression<Func<T, bool>> expression);

        Task<int> CountAsync(Expression<Func<T, bool>> expression);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);

        IEnumerable<T> Where(Expression<Func<T, bool>> expression);

        Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> expression);

        bool Any(Expression<Func<T, bool>> expression);

        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        T? SingleOrDefault(Expression<Func<T, bool>> expression);

        Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> expression);

        T? FirstOrDefault(Expression<Func<T, bool>> expression);

        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);

        T? Find(Expression<Func<T, bool>> expression);

        Task<T?> FindAsync(Expression<Func<T, bool>> expression);

        IEnumerable<T> ToList();

        Task<IEnumerable<T>> ToListAsync();
    }
}
