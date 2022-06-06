using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Common.Base;

namespace Chat.Database.Repository.Base
{
    public interface IBaseRepository<TModel> where TModel : BaseModel
    {
         TModel GetOne(Func<TModel, bool> predicate);
         Task Create(TModel item);
         IQueryable<TModel> GetAllObjects(bool trackChanges);

         IQueryable<TModel> FindByCondition(Expression<Func<TModel, bool>> expression,
             bool trackChanges);
         Task<TModel> GetById(Guid id);
         Task<TModel> Update(TModel item);
         Task<List<TModel>> UpdateRange(List<TModel> item);
         Task<TModel> Delete(Guid id);
    }
}