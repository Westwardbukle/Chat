using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Chat.Common.Converter;
using Chat.Common.Validating;

namespace Chat.Common.Dto
{
    public class RegisterUserDto : IValidatableObject
    {
        [Required] 
        public string Nickname { get; set; }
        
        [JsonConverter(typeof(NullableDateTimeConverter))]
        [Required] 
        public DateTime DateOfBirth { get; set; }
        
        [Required]
        [EmailAddress] 
        public string Email { get; set; }
        
        [Required]
        [RegularExpression(Consts.PasswordPattern, ErrorMessage = Consts.ErrorPassword)]
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            if (DateOfBirth >= DateTime.Today)
            {
                errors.Add(new ValidationResult("Date of birth cannot be later than today"));
            }
            
            return errors;
        }
    }
}