using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Chat.Database.AbstractRepository;
using Chat.Database.Model;

namespace Chat.Database.Repository
{
    public class CodeRepository : BaseRepository<CodeModel>, ICodeRepository
    {
        public CodeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<CodeModel>  GetCode(Expression<Func<CodeModel, bool>> predicate)
            => await GetOne(predicate);

        public async Task CreateCode(CodeModel item)
            => await CreateAsync(item);
        
        public void DeleteCode(CodeModel item)
            => Delete(item);
    }
}