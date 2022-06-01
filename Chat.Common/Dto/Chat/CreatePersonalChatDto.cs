using System;
using System.ComponentModel.DataAnnotations;

namespace Chat.Common.Dto.Chat
{
    public class CreatePersonalChatDto
    {
        [Required]
        public Guid User1 { get; set; }
        
        [Required]
        public Guid User2 { get; set; }
    }
}