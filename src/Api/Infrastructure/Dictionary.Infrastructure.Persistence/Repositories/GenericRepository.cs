using System.Linq.Expressions;
using Dictionary.Api.Application.Interfaces.Repositories;
using Dictionary.Api.Domain.Models;
using Dictionary.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Dictionary.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext _dbContext;


        protected DbSet<TEntity> _entity => _dbContext.Set<TEntity>();


        public GenericRepository(DictionaryContext dbContext)
        {
            _dbContext = dbContext;
        }


        #region Implementation of IGenericRepository<TEntity>


        #region Insert Methods


        public virtual async Task<int> AddAsync(TEntity entity)
        {
            await _entity.AddAsync(entity);
            return await _dbContext.SaveChangesAsync();
        }


        public virtual int Add(TEntity entity)
        {
            _entity.Add(entity);
            return _dbContext.SaveChanges();
        }


        public virtual int Add(IEnumerable<TEntity> entities)
        {
            if (entities != null && !entities.Any())
                return 0;

            _entity.AddRange(_entity);
            return _dbContext.SaveChanges();
        }


        public virtual async Task<int> AddAsync(IEnumerable<TEntity> entities)
        {
            if (entities != null && !entities.Any())
                return 0;

            await _entity.AddRangeAsync(entities);
            return await _dbContext.SaveChangesAsync();
        }


        #endregion


        #region Update Methods


        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            _entity.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;

            return await _dbContext.SaveChangesAsync();
        }


        public virtual int Update(TEntity entity)
        {
            _entity.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;

            return _dbContext.SaveChanges();
        }

        #endregion


        #region Delete Methods


        public virtual Task<int> DeleteAsync(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _entity.Attach(entity);
            }

            _entity.Remove(entity);

            return _dbContext.SaveChangesAsync();
        }


        public virtual int Delete(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                _entity.Attach(entity);
            }

            _entity.Remove(entity);

            return _dbContext.SaveChanges();
        }


        public virtual Task<int> DeleteAsync(Guid id)
        {
            var entity = _entity.Find(id);
            return DeleteAsync(entity);
        }


        public virtual int Delete(Guid id)
        {
            var entity = _entity.Find(id);
            return Delete(entity);
        }


        public virtual bool DeleteRange(Expression<Func<TEntity, bool>> predicate)
        {
            _dbContext.RemoveRange(_entity.Where(predicate));
            return _dbContext.SaveChanges() > 0;
        }


        public virtual async Task<bool> DeleteRangeAsync(Expression<Func<TEntity, bool>> predicate)
        {
            _dbContext.RemoveRange(_entity.Where(predicate));
            return await _dbContext.SaveChangesAsync() > 0;
        }


        #endregion


        #region AddOrUpdate Methods


        public virtual Task<int> AddOrUpdateAsync(TEntity entity)
        {
            // check the entity with the id already tracked
            if (_entity.Local.Any(i => EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
                _dbContext.Update(entity);

            return _dbContext.SaveChangesAsync();
        }


        public virtual int AddOrUpdate(TEntity entity)
        {
            if (_entity.Local.Any(i => EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
                _dbContext.Update(entity);

            return _dbContext.SaveChanges();
        }


        #endregion


        #region Get Methods


        public IQueryable<TEntity> AsQueryable() => _entity.AsQueryable();


        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _entity.AsQueryable();

            if(predicate !=null)
                query=query.Where(predicate);

            query = ApplyIncludes(query, includes);

            if (noTracking)
                query = query.Where(predicate);

            return query;
        }


        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            return Get(predicate, noTracking, includes).FirstOrDefaultAsync();
        }


        public virtual async Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _entity;

            if(predicate!=null)
                query=query.Where(predicate);

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);
            if (orderBy != null)
                query = orderBy(query);
            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }


        #endregion
        
        public virtual async Task<List<TEntity>> GetAll(bool noTracking = true)
        {
            if (noTracking)
                return await _entity.AsNoTracking().ToListAsync();

            return await _entity.ToListAsync();
        }

        public virtual async  Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, Func<IQueryable<TEntity>>? orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _entity;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includes.Any())
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.AsNoTracking().ToListAsync();
        }
        
        public virtual async Task<TEntity> GetByIdAsync(Guid id, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            TEntity found = await _entity.FindAsync(id);

            if (found == null)
                return null;

            if (noTracking)
                _dbContext.Entry(found).State = EntityState.Detached;

            foreach (Expression<Func<TEntity, object>> include in includes)
                _dbContext.Entry(found).Reference(include).Load(); //Lazy loading olarak degeri geri dönüyoruz

            return found;
        }

        public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _entity;

            if (predicate != null)
                query = query.Where(predicate);

            query = ApplyIncludes(query, includes);

            if (noTracking)
                query = query.AsNoTracking();

            return await query.SingleOrDefaultAsync();
        }





        #region Bulk Methods

        public virtual Task BulkDeleteById(IEnumerable<Guid> ids)
        {
            if (ids != null && !ids.Any())
                return Task.CompletedTask;

            _dbContext.RemoveRange(_entity.Where(i => ids.Contains(i.Id)));
            return _dbContext.SaveChangesAsync();
        }

        public virtual Task BuldDelete(Expression<Func<TEntity, bool>> predicate)
        {
            _dbContext.RemoveRange(_entity.Where(predicate));
            return _dbContext.SaveChangesAsync();
            
        }
        

        public virtual Task BulkDelete(IEnumerable<TEntity> entities)
        {
            if (entities != null && !entities.Any())
                return Task.CompletedTask;

            _entity.RemoveRange(entities);
            return _dbContext.SaveChangesAsync();
        }


        public virtual Task BulkUpdate(IEnumerable<TEntity> entities)
        {
            if (entities != null && !entities.Any())
                return Task.CompletedTask;


            // _entity.UpdateRange(entities); foreach yerine kisaca böylede yapabiliriz
            foreach (var entityItem in entities)
            {
                _entity.Update(entityItem);
               
            }

            return _dbContext.SaveChangesAsync();
        }


        public virtual async Task BulkAdd(IEnumerable<TEntity> entities)
        {
            if (entities != null && !entities.Any())
                await Task.CompletedTask;

            await _entity.AddRangeAsync(entities);

            await _dbContext.SaveChangesAsync();
        }



        #endregion
        
        #region SaveChanges Methods

        public Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        #endregion

        private static IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes)
        {
            if (includes != null)
            {
                foreach (var includeItem in includes)
                {
                    query = query.Include(includeItem);
                }
            }

            return query;
        }

        #endregion
    }
}