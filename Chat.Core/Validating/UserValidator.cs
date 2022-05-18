using System.Threading.Tasks;
using Chat.Common.Dto;
using Chat.Common.Error;
using Chat.Core.User;
using Chat.Database.Repository.User;

namespace Chat.Core.Validating
{
    public class UserValidator : IUserValidator
    {
        private readonly IUserRepository _userRepository;

        public UserValidator
        (
            IUserRepository userRepository
        )
        {
            _userRepository = userRepository;
        }
        
        public bool Validate (RegisterUserDto registerUserDto)
        {
            if (!EmailValidator.IsEmailValid(registerUserDto.Email))
            {
                return false;
            }
            
            if (registerUserDto.Nickname==null)
            {
                return false;
            }
            
            var trueUser = _userRepository.GetOne(u => u.Nickname == registerUserDto.Nickname);
            
            if (trueUser==null)
            {
                return false;
            }

            if (trueUser.Nickname == registerUserDto.Nickname)
            {
                return false;
            }

            if (registerUserDto.Password==null)
            {
                return false;
            }

            if (registerUserDto.Age==null)
            {
                return false;
            }

            return true;
        }
    }
}