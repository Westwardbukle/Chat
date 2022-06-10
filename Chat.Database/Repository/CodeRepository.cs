using System;
using Chat.Database.AbstractRepository;
using Chat.Database.Model;

namespace Chat.Database.Repository
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