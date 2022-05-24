using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Chat.Common.Validating;

namespace Chat.Common.Dto.Login
{
    public class LoginUserDto
    {
        [Required] 
        public string Nickname { get; set; }

        [RegularExpression(Consts.PasswordPattern, ErrorMessage = Consts.ErrorPassword)]
        public string Password { get; set; }
        
    }
}