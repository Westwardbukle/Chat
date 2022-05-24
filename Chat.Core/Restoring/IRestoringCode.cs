using System.Threading.Tasks;
using Chat.Common.Dto.Code;
using Chat.Common.Result;
using Chat.Common.User;

namespace Chat.Core.Restoring
{
    public interface IRestoringCode
    {
        Task<ResultContainer<CodeResponseDto>> SendRestoringCode(UserDto userDto);
    }
}