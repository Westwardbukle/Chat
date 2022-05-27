using System;
using System.Collections.Generic;
using Chat.Common.Dto.User;

namespace Chat.Common.User
{
    public class UsersReturnDto
    {
        public string Nickname { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        
        public string Email { get; set; }
        
        public bool Active { get; set; }
    }
}