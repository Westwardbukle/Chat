using System;
using System.Threading.Tasks;
using Chat.Common.Dto.Code;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Core.Abstract
{
    public interface IRestoringCodeService
    {
        Task<ActionResult> SendRestoringCode(Guid userId);
        Task<ActionResult> CodeСonfirmation(CodeDto codeDto);
    }
}