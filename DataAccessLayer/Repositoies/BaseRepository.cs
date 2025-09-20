using DataAccessLayer.Abstractions.IRepositories;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositoies
{
    public class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>, IDisposable where TEntity : class
    {
        private readonly DeliverySytemContext _context;

        public BaseRepository(DeliverySytemContext context)
            => _context = context;

        public void Dispose()
            => _context?.Dispose();

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>>? predicate = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> items = _context.Set<TEntity>().AsNoTracking(); // Importance Always include AsNoTracking for Query Side
            if (includeProperties != null)
                foreach (var includeProperty in includeProperties)
                    items = items.Include(includeProperty);

            if (predicate is not null)
                items = items.Where(predicate);

            return items;
        }

      

        public async Task<TEntity> FindSingleAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
            => await FindAll(null, includeProperties).AsTracking().SingleOrDefaultAsync(predicate, cancellationToken);

        public void Add(TEntity entity)
            => _context.Add(entity);

        public void Remove(TEntity entity)
            => _context.Set<TEntity>().Remove(entity);

        public void RemoveMultiple(List<TEntity> entities)
            => _context.Set<TEntity>().RemoveRange(entities);

        public void Update(TEntity entity)
            => _context.Set<TEntity>().Update(entity);
    }
}
