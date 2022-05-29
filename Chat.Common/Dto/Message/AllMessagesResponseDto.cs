using System;

namespace Chat.Common.Dto.Message
{
    public class AllMessagesResponseDto
    {
        public string Text { get; set; }
        
        public Guid UserId { get; set; }
        
        public Guid ChatId { get; set; }
        
        public DateTime DispatchTime { get; set; }
    }
}