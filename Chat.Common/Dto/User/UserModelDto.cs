using System;
using Chat.Common.Base;

namespace Chat.Common.Dto.User
{
    public class UserModelDto : BaseModel
    {
        public string Nickname { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        
        public string Email { get; set; }
        
        public string Password { get; set; }
    }
}