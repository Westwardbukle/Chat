using System;
using System.ComponentModel.DataAnnotations.Schema;
using Chat.Common.Base;
using Chat.Common.UsersRole;

namespace Chat.Database.Model
{
    public class UserChatModel : BaseModel
    {
        public Guid UserId { get; set; }
        
        public Guid ChatId { get; set; }
        
        public Role Role { get; set; }
        
        
        [ForeignKey(nameof(UserId))]
        public UserModel Users { get; set; }

        [ForeignKey(nameof(ChatId))]
        public ChatModel Chats { get; set; }
    }
}