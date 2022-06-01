using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;

namespace Chat.Core.Abstract
{
    public interface IMessageService
    {
        Task SendMessage(Guid userId, Guid chatId, string text);
        Task<List<MessagesResponseDto>> GetAllMessageInChat(Guid chatId);
        Task SendPersonalMessage(Guid senderId, Guid recipientId, string text);
        /*Task<ActionResult> GetAllMessagesFromUser();*/
        
    }
}