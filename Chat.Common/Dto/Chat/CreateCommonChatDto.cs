using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Chat.Common.Chat;

namespace Chat.Common.Dto.Chat
{
    public class CreateCommonChatDto
    {
        [Required]
        public Guid AdminId { get; set; }
        
        [Required]
        public string Name { get; set; }
    }
    
    public class InviteUserCommonChatDto
    {
        [Required]
        public List<Guid> UserIds { get; set; }
    }
}