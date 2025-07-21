using Dispatcher.DAL.Entities;
using System.Linq.Expressions;

namespace Dispatcher.DAL.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity 
    {
        Task CreateAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
