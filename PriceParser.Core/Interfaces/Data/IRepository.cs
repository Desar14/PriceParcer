using PriceParser.Data;
using PriceParser.Data.Entities;
using System.Linq.Expressions;

namespace PriceParser.Core
{
    public interface IRepository<TEntity> : IDisposable where TEntity : BaseEntity
    {
        public Task Add(TEntity entity);

        public Task AddRange(IEnumerable<TEntity> entity);

        public Task<IQueryable<TEntity>> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);
        public Task<TEntity?> GetByID(object id, params Expression<Func<TEntity, object>>[] includes);
        public Task<IEnumerable<TEntity>> GetWithRawSql(string query, params object[] parameters);

        public Task<IQueryable<TEntity>> GetQueryable();

        public Task<TEntity?> FindBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        public Task PatchAsync(Guid id, List<PatchModel> patchDtos);

        public Task Update(TEntity entityToUpdate);

        public void Delete(TEntity entityToDelete);
        public Task Delete(object id);


    }
}
