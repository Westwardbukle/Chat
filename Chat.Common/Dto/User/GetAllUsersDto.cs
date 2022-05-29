using System;
using System.Collections.Generic;
using Chat.Common.Base;
using Chat.Common.User;

namespace Chat.Common.Dto.User
{
    public class GetAllUsersDto 
    {
        public string Nickname { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        
        public string Email { get; set; }
        
    }
}