using System;
using System.ComponentModel.DataAnnotations;

namespace Chat.Common.Dto.Message
{
    public class PersonalMessageDto
    {
        [Required] 
        public string Text { get; set; }
    }
}