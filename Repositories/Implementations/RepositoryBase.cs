using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System.Linq.Expressions;

namespace Repositories.Implementations
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly RepositoryContext _context;

        public RepositoryBase(RepositoryContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<T>> FindAllAsync(bool trackChanges) =>
            !trackChanges
                ? _context.Set<T>().AsNoTracking()
                : _context.Set<T>();

        public async Task<IQueryable<T>> FindAllByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges
                ? _context.Set<T>().Where(expression).AsNoTracking()
                : _context.Set<T>().Where(expression);
        public async Task<T?> FindByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            return !trackChanges
                ? await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(expression)
                : await _context.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task CreateAsync(T entity) =>
            await _context.Set<T>().AddAsync(entity);

        public async Task RemoveAsync(T entity) =>
            _context.Set<T>().Remove(entity);

        public async Task UpdateAsync(T entity) =>
            _context.Set<T>().Update(entity);
    }

}