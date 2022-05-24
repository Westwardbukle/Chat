using System;
using System.ComponentModel.DataAnnotations;

namespace Chat.Common.User
{
    public class UserDto
    {
        [Required]
        public Guid Userid { get; set; }
    }
}