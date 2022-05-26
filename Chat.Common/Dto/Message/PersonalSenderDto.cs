using System;

namespace Chat.Common.Dto.Message
{
    public class PersonalSenderDto
    {
        public Guid SenderId { get; set; }
        
        public string Text { get; set; }
    }
}