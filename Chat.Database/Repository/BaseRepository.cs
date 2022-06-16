using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Common.Base;
using Microsoft.EntityFrameworkCore;

namespace Chat.Database.Repository
{
    public abstract class BaseRepository<TModel>
        where TModel : BaseModel
    {
        protected readonly AppDbContext AppDbContext;

        protected BaseRepository(AppDbContext appDbContext)
            => AppDbContext = appDbContext;

        /*public async Task<TModel>  GetOneToId(Guid id) 
            => await AppDbContext.Set<TModel>().AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);*/
            
        
        public TModel GetOne(Func<TModel, bool> predicate)
            => AppDbContext.Set<TModel>().AsNoTracking().FirstOrDefault(predicate);
        
        
        public IQueryable<TModel> GetAllObjects(bool trackChanges)
            => !trackChanges
                ? AppDbContext.Set<TModel>()
                    .AsNoTracking()
                : AppDbContext.Set<TModel>();


        public IQueryable<TModel> FindByCondition(Expression<Func<TModel, bool>> expression,
            bool trackChanges)
            => !trackChanges
                ? AppDbContext.Set<TModel>()
                    .Where(expression)
                    .AsNoTracking()
                : AppDbContext.Set<TModel>()
                    .Where(expression);


        public async Task CreateAsync(TModel item) => await AppDbContext.Set<TModel>().AddAsync(item);

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