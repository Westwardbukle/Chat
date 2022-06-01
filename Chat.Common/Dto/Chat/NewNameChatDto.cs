using System.ComponentModel.DataAnnotations;

namespace Chat.Common.Dto.Chat
{
    public class NewNameChatDto
    {
        [Required]
        public string Name { get; set; }
    }
}