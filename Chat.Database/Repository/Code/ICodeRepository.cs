using System;
using Chat.Database.Model;
using Chat.Database.Repository.Base;

namespace Chat.Database.Repository.Code
{
    public interface ICodeRepository
    {
        CodeModel GetCode(Func<CodeModel, bool> predicate);
        public void CreateCode(CodeModel item);
        public void DeleteCode(CodeModel item);
    }
}