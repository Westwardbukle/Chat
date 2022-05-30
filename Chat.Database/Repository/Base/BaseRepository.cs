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
        protected readonly AppDbContext AppDbContext;

        protected BaseRepository(AppDbContext appDbContext)
            => AppDbContext = appDbContext;

        public TModel GetOne(Func<TModel, bool> predicate)
            => AppDbContext.Set<TModel>().AsNoTracking().FirstOrDefault(predicate);
        
        public IQueryable<TModel> GetAllObjects(bool trackChanges)
            => !trackChanges ?
                AppDbContext.Set<TModel>()
                    .AsNoTracking() :
                AppDbContext.Set<TModel>();
        
        public IQueryable<TModel> FindByCondition(Expression<Func<TModel, bool>> expression,
            bool trackChanges) => 
            !trackChanges ?
                AppDbContext.Set<TModel>()
                    .Where(expression)
                    .AsNoTracking() :
                AppDbContext.Set<TModel>()
                    .Where(expression);
        
        public void Create(TModel item)
        {
            AppDbContext.Set<TModel>().Add(item);
            AppDbContext.SaveChangesAsync();
        }

        public void Update(TModel item) => AppDbContext.Set<TModel>().Update(item);
        
        public async Task<List<TModel>> UpdateRange(List<TModel> item)
        {
            AppDbContext.Set<TModel>().UpdateRange(item);
            await AppDbContext.SaveChangesAsync();
            return item;
        }

        public async Task<TModel> GetById(Guid id)
            => await AppDbContext.Set<TModel>().FindAsync(id);
        
        public void Delete(TModel item) => AppDbContext.Set<TModel>().Remove(item);
    }
}