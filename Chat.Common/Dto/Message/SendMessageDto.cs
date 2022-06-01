using System;
using System.ComponentModel.DataAnnotations;

namespace Chat.Common.Dto.Message
{
    public class SendMessageDto
    {
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        public string Text { get; set; }
    }
}