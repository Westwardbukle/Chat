using System.Threading.Tasks;
using Chat.Common.Dto.Code;
using Chat.Common.User;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Core.Restoring
{
    public interface IRestoringCodeService
    {
        Task<ActionResult> SendRestoringCode(UserDto userDto);
        Task<ActionResult> CodeСonfirmation(CodeDto codeDto);
    }
}