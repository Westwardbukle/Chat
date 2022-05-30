using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Common.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Chat.Database.Repository.Base
{
    public abstract class BaseRepository<TModel>
        where TModel : BaseModel
    {
        private readonly AppDbContext _context;
        private readonly DbSet<TModel> _dbSet;

        protected BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TModel>();
        }

        public TModel GetOne(Func<TModel, bool> predicate)
            => _context.Set<TModel>().AsNoTracking().FirstOrDefault(predicate);
        
        public IEnumerable<TModel> GetAllObjects()
            => _context.Set<TModel>().AsNoTracking().ToList();
        
        public IEnumerable<TModel> GetByFilter(Func<TModel, bool> predicate)
            => _context.Set<TModel>().Where(predicate);

        public IEnumerable<TModel> GetWithInclude(params Expression<Func<TModel, object>>[] includeProperties)
        {
            return Include(includeProperties);
        }
 
        public IEnumerable<TModel> GetWithInclude(Func<TModel,bool> predicate, 
            params Expression<Func<TModel, object>>[] includeProperties)
        {
            var query =  Include(includeProperties);
            return query.AsEnumerable().Where(predicate);
        }
 
        private IQueryable<TModel> Include(params Expression<Func<TModel, object>>[] includeProperties)
        {
            var query = _dbSet.AsNoTracking();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
        

        public async Task<TModel> Create(TModel item)
        {
            item.DateCreated = DateTime.Now;
            await _context.Set<TModel>().AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }
        
        public async Task<TModel> Update(TModel item)
        {
            _context.Set<TModel>().Update(item);
            await _context.SaveChangesAsync();
            return item;
        }
        
        public async Task<List<TModel>> UpdateRange(List<TModel> item)
        {
            _context.Set<TModel>().UpdateRange(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<TModel> GetById(Guid id)
            => await _context.Set<TModel>().FindAsync(id);
        
        public async Task<TModel> Delete(Guid id)
        {
            var item = await _context.Set<TModel>().FindAsync(id);
            
            if (item == null)
                return null;
            
            _context.Set<TModel>().Remove(item);
            
            await _context.SaveChangesAsync();
            
            return item;
        }
    }
}