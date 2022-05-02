using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PriceParser.Core;
using PriceParser.Data;
using PriceParser.Data.Entities;
using System.Linq.Expressions;

namespace PriceParser.DataAccess
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;
        protected readonly ILogger<Repository<T>> _logger;

        public Repository(ApplicationDbContext db, ILogger<Repository<T>> logger)
        {
            _dbContext = db;
            _dbSet = _dbContext.Set<T>();
            _logger = logger;
        }

        public virtual async Task Add(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository {nameof(T)} add error");
                throw;
            }

        }

        public virtual async Task AddRange(IEnumerable<T> entity)
        {
            try
            {
                await _dbSet.AddRangeAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository {nameof(T)} add range error");
                throw;
            }
        }

        public virtual void Delete(T entityToDelete)
        {
            try
            {
                if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
                {
                    _dbSet.Attach(entityToDelete);
                }
                _dbSet.Remove(entityToDelete);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository {nameof(T)} delete entity error");
                throw;
            }
        }

        public virtual async Task Delete(object id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                    throw new Exception();
                else
                    _dbSet.Remove(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository {nameof(T)} delete by id error");
                throw;
            }
        }



        public virtual async Task<IQueryable<T>> Get(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (includes.Any())
                {
                    query = includes.Aggregate(query, (current, include) => current.Include(include));
                }


                if (orderBy != null)
                {
                    return orderBy(query);
                }
                else
                {
                    return query;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository {nameof(T)} get error");
                throw;
            }
        }

        public virtual async Task<T?> GetByID(object id, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                var query = _dbSet.AsNoTracking();

                if (includes.Any())
                {
                    query = includes.Aggregate(query, (current, include) => current.Include(include));
                }

                return await query
                    .FirstOrDefaultAsync(entity => entity.Id.Equals(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository {nameof(T)} get by id error");
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> GetWithRawSql(string query, params object[] parameters)
        {
            try
            {
                return _dbSet.FromSqlRaw(query, parameters).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository {nameof(T)} get with raw sql error");
                throw;
            }
        }

        public async Task<T?> FindBy(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (includes.Any())
                {
                    query = includes.Aggregate(query, (current, include) => current.Include(include));
                }

                return await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository {nameof(T)} find by error");
                throw;
            }
        }

        public virtual async Task PatchAsync(Guid id, List<PatchModel> patchDtos)
        {
            try
            {
                var model = await _dbSet.FirstOrDefaultAsync(entity => entity.Id.Equals(id));

                var nameValuePairProperties = patchDtos
                    .ToDictionary(a => a.PropertyName, a => a.PropertyValue);

                var dbEntityEntry = _dbContext.Entry(model);
                dbEntityEntry.CurrentValues.SetValues(nameValuePairProperties);
                dbEntityEntry.State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository {nameof(T)} patch error");
                throw;
            }
        }

        public virtual Task Update(T entityToUpdate)
        {
            try
            {
                _dbSet.Update(entityToUpdate);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Repository {nameof(T)} update error");
                throw;
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<IQueryable<T>> GetQueryable()
        {
            return _dbSet;
        }
    }
}
