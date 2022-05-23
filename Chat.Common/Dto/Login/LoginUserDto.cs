using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Chat.Core.Validating;

namespace Chat.Common.Dto.Login
{
    public class LoginUserDto : IValidatableObject
    {
        [Required] 
        public string Nickname { get; set; }

        [RegularExpression(Consts.PasswordPattern, ErrorMessage = Consts.ErrorPassword)]
        public string Password { get; set; }
        
        [Required]
        public string LoginCode { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();
            
            return errors;
        }
    }
}