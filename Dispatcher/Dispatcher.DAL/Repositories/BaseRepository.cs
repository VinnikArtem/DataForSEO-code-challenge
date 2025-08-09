using Dispatcher.DAL.EF;
using Dispatcher.DAL.Entities;
using Dispatcher.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Dispatcher.DAL.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ApplicationContext _applicationContext;
        private readonly DbSet<TEntity> _entities;

        public BaseRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _entities = _applicationContext.Set<TEntity>();
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            IQueryable<TEntity> query = _entities.AsNoTracking();

            if (include != null) query = include(query);

            return await query.ToListAsync();
        }

        public async Task<TEntity> GetFirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            if (predicate == null) return default;

            IQueryable<TEntity> query = _entities.AsNoTracking();

            if (include != null) query = include(query);

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            var item = await _entities.FindAsync(entity.Id);

            if (item == null) return;

            foreach (var navigationEntity in _applicationContext.Entry(entity).Navigations)
            {
                if (navigationEntity.CurrentValue != null)
                {
                    var navigationEntityName = navigationEntity.Metadata.Name;

                    var navigationItem = _applicationContext.Entry(item).Navigation(navigationEntityName);

                    navigationItem.Load();

                    navigationItem.CurrentValue = navigationEntity.CurrentValue;
                }
            }

            _applicationContext.Entry(item).CurrentValues.SetValues(entity);
        }
    }
}
