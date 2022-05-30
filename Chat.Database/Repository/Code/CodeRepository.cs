using System;
using System.Threading.Tasks;
using Chat.Database.Model;
using Chat.Database.Repository.Base;

namespace Chat.Database.Repository.Code
{
    public class CodeRepository : BaseRepository<CodeModel>, ICodeRepository
    {
        public CodeRepository(AppDbContext context) : base(context)
        {
        }

        public CodeModel GetCode(Func<CodeModel, bool> predicate)
            => GetOne(predicate);

        public void CreateCode(CodeModel item)
            => Create(item);
        
        public void DeleteCode(CodeModel item)
            => Delete(item);
    }
}