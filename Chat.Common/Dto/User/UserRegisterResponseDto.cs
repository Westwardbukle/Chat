using System;

namespace Chat.Common.Dto.User
{
    public class UserRegisterResponseDto 
    {
        public Guid Id { get; set; }
        
        public string Nickname { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        
        public string Email { get; set; }
    }
}