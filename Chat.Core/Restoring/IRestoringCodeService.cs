using System.Threading.Tasks;
using Chat.Common.Dto.Code;
using Chat.Common.Result;
using Chat.Common.User;

namespace Chat.Core.Restoring
{
    public interface IRestoringCodeService
    {
        Task<ResultContainer<CodeResponseDto>> SendRestoringCode(UserDto userDto);
        Task<ResultContainer<CodeResponseDto>> CodeСonfirmation(CodeDto codeDto);
    }
}