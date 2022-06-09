using System;
using Chat.Common.UsersRole;

namespace Chat.Common.Dto.UserChat
{
    public class UserChatResponseDto
    {
        public Guid Id { get; set; }
        
        public Guid UserId { get; set; }
        
        public Guid ChatId { get; set; }
        
        public Role Role { get; set; }
    }
}