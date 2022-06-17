using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Database.Model;

namespace Chat.Database.AbstractRepository
{
    public interface ICodeRepository
    {
        Task<CodeModel> GetCode(Expression<Func<CodeModel, bool>> predicate);
        Task CreateCode(CodeModel item);
        public void DeleteCode(CodeModel item);
    }
}