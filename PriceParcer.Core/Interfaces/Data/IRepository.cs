using Microsoft.AspNetCore.Identity;
using PriceParcer.Data;
using PriceParcer.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PriceParcer.Core
{
    public interface IRepository<TEntity> : IDisposable where TEntity : BaseEntity
    {
        public Task Add(TEntity entity);

        public Task AddRange(IEnumerable<TEntity> entity);

        public Task<IEnumerable<TEntity>> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);
        public Task<TEntity?> GetByID(object id);
        public Task<IEnumerable<TEntity>> GetWithRawSql(string query, params object[] parameters);

        public Task<TEntity?> FindBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        public Task PatchAsync(Guid id, List<PatchModel> patchDtos);

        public Task Update(TEntity entityToUpdate);

        public void Delete(TEntity entityToDelete);
        public Task Delete(object id);

        
    }
}
