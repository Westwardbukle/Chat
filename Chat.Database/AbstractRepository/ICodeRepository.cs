using System;
using Chat.Database.Model;

namespace Chat.Database.AbstractRepository
{
    public interface ICodeRepository
    {
        CodeModel GetCode(Func<CodeModel, bool> predicate);
        public void CreateCode(CodeModel item);
        public void DeleteCode(CodeModel item);
    }
}