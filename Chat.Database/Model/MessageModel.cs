using System;
using System.ComponentModel.DataAnnotations.Schema;
using Chat.Common.Base;

namespace Chat.Database.Model
{
    public class MessageModel : BaseModel
    {
        public string Text { get; set; }
        
        public Guid UserId { get; set; }
        
        public Guid ChatId { get; set; }
        
        public DateTime DispatchTime { get; set; }
        
        [ForeignKey(nameof(UserId))]
        public UserModel Users { get; set; }
        
        [ForeignKey(nameof(ChatId))]
        public ChatModel Chats { get; set; }
    }
}