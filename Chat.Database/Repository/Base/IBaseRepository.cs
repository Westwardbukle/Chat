using System;
using System.Threading.Tasks;
using Chat.Common.Base;

namespace Chat.Database.Repository.Base
{
    public interface IBaseRepository<TModel> where TModel : BaseModel
    {
         TModel GetOne(Func<TModel, bool> predicate);
         Task<TModel> Create(TModel item);
         Task<TModel> GetById(Guid id);
         Task<TModel> Update(TModel item);
         Task<TModel> Delete(Guid id);
    }
}