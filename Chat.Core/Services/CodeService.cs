using System;
using Chat.Core.Code;

namespace Chat.Core.Services
{
    public class CodeService : ICodeService
    {
        public string GenerateRestoringCode()
        {
            var random = new Random();

            return random.Next(100000, 999999).ToString();
        }
    }
}