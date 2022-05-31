using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.Common.Dto.Message;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Core.Message
{
    public interface IMessageService
    {
        Task SendMessage(Guid userId, Guid chatId, string text);
        Task<List<AllMessagesResponseDto>> GetAllMessageInChat(Guid chatId);
        /*Task<ActionResult> SendPersonalMessage(Guid senderId, Guid recipientId, string text);*/
        /*Task<ActionResult> GetAllMessagesFromUser();*/
        
    }
}