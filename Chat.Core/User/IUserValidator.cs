using Chat.Common.Dto;

namespace Chat.Core.User
{
    public interface IUserValidator
    {
        public bool Validate(RegisterUserDto registerUserDto);
    }
}