using Chat.Database.Model;
using Chat.Database.Repository.Base;

namespace Chat.Database.Repository.Code
{
    public class CodeRepository : BaseRepository<CodeModel>, ICodeRepository
    {
        public CodeRepository(AppDbContext context) : base(context)
        {
        }
    }
}