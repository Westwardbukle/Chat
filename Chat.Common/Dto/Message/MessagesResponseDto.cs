using System;
using Chat.Common.Message;

namespace Chat.Common.Dto.Message
{
    public class MessagesResponseDto
    {
        public string Text { get; set; }
        
        public Guid UserId { get; set; }
        
        public Guid ChatId { get; set; }
        
        public MessageType Type { get; set; }
        
        public DateTime DispatchTime { get; set; }
    }
}