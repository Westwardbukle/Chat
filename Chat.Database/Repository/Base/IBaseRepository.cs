using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Common.Base;

namespace Chat.Database.Repository.Base
{
    public interface IBaseRepository<TModel> where TModel : BaseModel
    {
         TModel GetOne(Func<TModel, bool> predicate);
         Task<TModel> Create(TModel item);
         IEnumerable<TModel> GetAllObjects();
         IEnumerable<TModel> GetByFilter(Func<TModel, bool> predicate);
         Task<TModel> GetById(Guid id);
         Task<TModel> Update(TModel item);
         Task<List<TModel>> UpdateRange(List<TModel> item);
         Task<TModel> Delete(Guid id);
         IEnumerable<TModel> GetWithInclude(params Expression<Func<TModel, object>>[] includeProperties);

         IEnumerable<TModel> GetWithInclude(Func<TModel, bool> predicate,
             params Expression<Func<TModel, object>>[] includeProperties);
    }
}