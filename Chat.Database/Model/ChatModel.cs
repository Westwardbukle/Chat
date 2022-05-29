#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Chat.Common.Base;
using Chat.Common.Chat;

namespace Chat.Database.Model
{
    [Table("Chat")]
    public class ChatModel : BaseModel
    {
        public string? Name { get; set; }
        
        public ChatType Type { get; set; }
        
        public ICollection<UserChatModel> UserChats { get; set; }
        
        public ICollection<MessageModel> MessageModels { get; set; }
    }
}