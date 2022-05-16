using System.Threading.Tasks;
using Chat.Common.Dto;
using Chat.Common.Result;

namespace Chat.Core.Services
{
    public class AuthService
    {
        public Task<ResultContainer<UserResponseDto>> Registration(UserDto userDto)
        {
            var result = new ResultContainer<UserResponseDto>();
            
            
            
            return result;
        }
        
        
    }
}