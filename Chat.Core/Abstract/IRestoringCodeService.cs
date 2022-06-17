using System;
using System.Threading.Tasks;
using Chat.Common.Dto.Code;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Core.Abstract
{
    public interface IRestoringCodeService
    {
        Task SendRestoringCodeAsync(Guid userId);
        Task ConfirmEmailCodeAsync(CodeDto codeDto);
    }
}