using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Chat.Database.Repository.User;

namespace Chat.Common.Dto
{
    public class RegisterUserDto /*: IValidatableObject*/
    {
        private readonly IUserRepository _userRepository;

        public RegisterUserDto
        (
            IUserRepository userRepository
        )
        {
            _userRepository = userRepository;
        }
            
        [Required]
        public string Nickname { get; set; }
        
        [Required]
        public DateTime DateOfBirth { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }

        /*public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errerMessage = $"nickname exists{UserNickname.Exists}";
            


            var trueUser = _userRepository.GetOne(u => u.Nickname == RegisterUserDto.Nickname);
            
        }*/
    }
}