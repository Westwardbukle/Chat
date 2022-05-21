using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Chat.Core.Validating;

namespace Chat.Common.Dto.Login
{
    public class LoginUserDto : IValidatableObject
    {
        [Required] 
        public string Nickname { get; set; }

        [Required] 
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();
            
            if (!Regex.IsMatch(Password, Consts.PasswordPattern))
            {
                errors.Add(new ValidationResult
                ("Хотя бы одна цифра [0-9] "
                 + "Хотя бы один символ нижнего регистра [a-z] "
                 + "Хотя бы один символ верхнего регистра [A-Z] "
                 + "Хотя бы один специальный символ [*.!@#$%^&(){}[]:;<>,.?/~_+-=|\\] "
                 + "Длина не менее 8 символов, но не более 32. "
                ));
            }

            return errors;
        }
    }
}